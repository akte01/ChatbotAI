import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatCardModule} from '@angular/material/card';
import {MatListModule} from '@angular/material/list';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormsModule} from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import {MatButtonModule} from '@angular/material/button';
import { ChatbotAIApiClient, ChatbotAIApiBaseUrl } from './shared/services/chatbot-api-client';
import { ChatbotService } from './shared/services/chatbot.service';
@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NoopAnimationsModule,
    MatCardModule,
    MatListModule,
    MatFormFieldModule,
    MatIconModule,
    FormsModule,
    MatInputModule,
    HttpClientModule,
    MatButtonModule
  ],
  providers: [
    ChatbotService,
    ChatbotAIApiClient,
    {
      provide: ChatbotAIApiBaseUrl,
      useValue: "https://localhost:44338"
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
