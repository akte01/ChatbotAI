import { Injectable } from '@angular/core';
import {
  CancelResponseCommand,
  ChatbotAIApiClient,
  GenerateResponseCommand,
  Message,
  SaveFeedbackCommand,
} from './chatbot-api-client';
import {
  BehaviorSubject,
  Observable,
  concatMap,
  finalize,
  from,
  interval,
  map,
  of,
  scan,
  switchMap,
  take,
  takeWhile,
  tap,
} from 'rxjs';
import { Sender } from '../enums/sender.enum';

@Injectable()
export class ChatbotService {
  private messagesSource: BehaviorSubject<Message[]> = new BehaviorSubject<
    Message[]
  >([]);
  private breakMessageSource: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );
  private finishedTypingSource: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    true
  );

  constructor(private apiClient: ChatbotAIApiClient) {

  }

  get messages$(): Observable<Message[]> {
    return this.messagesSource.asObservable();
  }

  get finishedTyping$(): Observable<boolean> {
    return this.finishedTypingSource.asObservable();
  }

  getMessages(): Observable<Message[]> {
    return this.apiClient
      .getMessages()
      .pipe(tap((messages) => this.messagesSource.next(messages)));
  }

  cancelResponse(): Observable<any> {
    this.breakMessageSource.next(true);
    return this.finishedTypingSource.pipe(
      switchMap((isCanceled) => {
        if (isCanceled) {
          const messages = this.messagesSource.getValue();
          const lastMessage =  messages[messages.length - 1];
          return this.apiClient.cancelResponse({
            messageId: lastMessage?.messageId,
            content: lastMessage?.content,
          } as CancelResponseCommand);
        }
        return of(null);
      })
    );
  }

  sendMessage(newMessage: string): Observable<any> {
    this.breakMessageSource.next(false);
    this.finishedTypingSource.next(false);
    const messages = this.messagesSource.getValue();
    const date = new Date();
    var userMessage = new GenerateResponseCommand({
      content: newMessage,
      date: date,
    });
    messages.push(
      new Message({
        content: newMessage,
        date: date,
        sender: Sender.User,
      })
    );
    return this.apiClient.generateResponse(userMessage).pipe(
      tap(() => {
        this.messagesSource.next(messages);
      }),
      switchMap((response) => this.generateMessage(response))
    );
  }

  vote(messageId: number | undefined, grade: number): Observable<any> {
    const messages = this.messagesSource.getValue();
    let index = messages.findIndex((m) => m.messageId === messageId);
    messages[index].grade = grade;
    return this.apiClient
      .saveFeedback(new SaveFeedbackCommand({ messageId: messageId, grade: grade }))
      .pipe(tap(() => this.messagesSource.next(messages)));
  }

  private generateMessage(response: Message): Observable<any> {
    return of(response).pipe(
      map((response) => {
        const messages = this.messagesSource.getValue();
        const content = response.content;
        response.content = '';
        messages.push(response);
        this.messagesSource.next(messages);
        return content || '';
      }),
      switchMap((content) =>
        this.getTypewriterEffect(content).pipe(
          tap((typedContent) => {
            this.updateContent(typedContent, response.messageId || 0);
          })
        )
      )
    );
  }

  private updateContent(content: string, messageId: number): void {
    const messages = this.messagesSource.getValue();
    let index = messages.findIndex((m) => m.messageId === messageId);
    messages[index].content = content;
    this.messagesSource.next(messages);
  }

  private type(word: string): Observable<string> {
    return interval(5).pipe(
      map((x) => word.substring(0, x + 1)),
      take(word.length)
    );
  }

  private getTypewriterEffect(response: string): Observable<string> {
    return from(response).pipe(
      concatMap((title) => this.type(title)),
      takeWhile(() => this.breakMessageSource.getValue() === false),
      scan((total, res) => total + res),
      finalize(() => {
        this.finishedTypingSource.next(true);
      })
    );
  }
}
