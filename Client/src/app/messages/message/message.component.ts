import { Component, OnInit, OnDestroy } from '@angular/core';
import { MessageService } from "../../_services/message.service";
import { AccountService } from "../../_services/account.service";
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit, OnDestroy {

  receiver: string = '';

  messages: any = [];
  newMessage: string = '';

  Content: any = {
    sender: null,
    receiver: null,
    content: null,
  };

  currentUser: any;
  socketSubscription!: Subscription;

  currentPage: number = 1;
  listPerPage: number = 10;
  totalMessage: number = 0;
  totalPage: number = 0;

  constructor(private messageService: MessageService, private accountService: AccountService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.receiver = params['username'];
    });
    this.currentUser = this.accountService.getCurrentUser();

    this.refreshList();



    // Store the subscription
    this.socketSubscription = this.messageService.connect('wss://localhost:7173/ws').subscribe(
      (message: any) => {
        this.messages.push(message);
      },
      (error: any) => {
        console.error('Error in WebSocket connection:', error);
      },
      () => {
        console.log('WebSocket connection closed.');
      }
    );
  }

  refreshList()
  {
    this.messageService.getNumberOfMessage(this.currentUser.username, this.receiver).subscribe({
      next: totalList => {
        this.totalMessage = totalList;
        this.totalPage = Math.floor((this.totalMessage + this.listPerPage - 1) / this.listPerPage);

        console.log(this.totalPage);

        this.messageService.getMessages(this.currentUser.username, this.receiver, this.currentPage, this.listPerPage)
          .subscribe({
            next: (data) => {
              this.messages = data;
            },
            error: (err) => {
              console.log(err);
            },
          });
      },
      error: err => {
        console.log(err);
      }
    });
  }
  ngOnDestroy() {
    // Unsubscribe from the WebSocket connection when the component is destroyed
    if (this.socketSubscription) {
      this.socketSubscription.unsubscribe();
    }
  }

  OnClick() {
    if (this.newMessage != '') {
      this.Content.sender = this.currentUser.username;
      this.Content.receiver = this.receiver;
      this.Content.content = this.newMessage;
      this.messageService.sendMessageToWS(this.Content);
      this.messageService.sendMessage(this.Content).subscribe({
        next: response => {
          //console.log(response);
        }
      });

      this.newMessage = '';
    }
  }
  onPageChange(pageNumber: number): void {
    this.currentPage = pageNumber;
    this.refreshList();
  }
}
