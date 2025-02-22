import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Sender } from './shared/enums/sender.enum';
import { Grade } from './shared/enums/grade.enum';
import { Message } from './shared/services/chatbot-api-client';
import { ChatbotService } from './shared/services/chatbot.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChildren('messages') messagesElements: QueryList<any> =
    {} as QueryList<any>;
  @ViewChild('content') contentElement: ElementRef = {} as ElementRef;

  subscriptions: Subscription = new Subscription();
  messages$: Observable<Message[]>;
  disabled$: Observable<boolean>;
  newMessage: string = '';
  gradeType = Grade;
  senderType = Sender;

  constructor(private chatbotService: ChatbotService) {
    this.messages$ = this.chatbotService.messages$;
    this.disabled$ = this.chatbotService.finishedTyping$;
  }

  ngOnInit(): void {
    const subs = this.chatbotService.getMessages().subscribe();
    this.subscriptions.add(subs);
  }

  ngAfterViewInit(): void {
    const subs = this.messagesElements.changes.subscribe(this.scrollToBottom);
    this.subscriptions.add(subs);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  sendMessage(): void {
    const subs = this.chatbotService
      .sendMessage(this.newMessage)
      .subscribe(() => {
        this.newMessage = '';
        this.scrollToBottom();
      });
    this.subscriptions.add(subs);
  }

  vote(messageId: number | undefined, grade: number): void {
    const subs = this.chatbotService.vote(messageId, grade).subscribe();
    this.subscriptions.add(subs);
  }

  cancelResponse(): void {
    const subs = this.chatbotService.cancelResponse().subscribe();
    this.subscriptions.add(subs);
  }

  private scrollToBottom = () => {
    try {
      this.contentElement.nativeElement.scrollTop =
        this.contentElement.nativeElement.scrollHeight;
    } catch (err) {}
  };
}
