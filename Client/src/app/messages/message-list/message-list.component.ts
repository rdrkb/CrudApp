import {Component, OnInit} from '@angular/core';
import {MessageService} from "../../_services/message.service";
import {AccountService} from "../../_services/account.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-message-list',
  templateUrl: './message-list.component.html',
  styleUrl: './message-list.component.css'
})
export class MessageListComponent implements OnInit {

  messages:any = [];
  currentUser: any;

  currentPage: number = 1;
  listPerPage: number = 2;
  totalList: number = 0;
  totalPage: number = 0;

  constructor(private messageService: MessageService, private accountService: AccountService,private router: Router) {
    this.currentUser = this.accountService.getCurrentUser();

  }
  ngOnInit() {
    this.refreshList();
  }

  refreshList(){
    this.messageService.getNumberOfMessageList(this.currentUser.username).subscribe({
      next: totalList => {
        this.totalList = totalList;
        this.totalPage = Math.floor((this.totalList + this.listPerPage - 1) / this.listPerPage);

        this.messageService.getMessageList(this.currentUser.username, this.currentPage, this.listPerPage).subscribe({
          next: data => {
            this.messages = data;
          },
          error: err => {
            console.log(err);
          }
        });
      },
      error: err => {
        console.log(err);
      }
    });
  }

  onPageChange(pageNumber: number): void {
    this.currentPage = pageNumber;
    this.refreshList();
  }
  seeMessage(message: any) {
    var friend = message.sender === this.currentUser.username ? message.receiver : message.sender;
    this.router.navigate(['/message', friend]);
  }
}
