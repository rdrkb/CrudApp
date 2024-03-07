import {Component, OnDestroy, OnInit} from '@angular/core';
import {AdminService} from "../../_services/admin.service";
import {AccountService} from "../../_services/account.service";
import {Router} from "@angular/router";
import {NotificationService} from "../../_services/notification.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent implements OnInit, OnDestroy {

  notificationList: any = [];
  currentUser!: string;


  socketSubscription!: Subscription;

  currentPage: number = 1;
  itemPerPage: number = 5;
  totalItems: number = 0;
  totalPages: number = 0;

  constructor(private notificationService: NotificationService, private accountService: AccountService, private router: Router) {}

  ngOnInit() {
    this.currentUser = this.accountService.getCurrentUser();
    this.refreshList();

    // Store the subscription
    this.socketSubscription = this.notificationService.connect('wss://localhost:7290/ws/notification').subscribe(
      (notification: any) => {

        this.notificationList.push(notification);

        if(this.currentPage > 1 || this.notificationList.length > this.itemPerPage) {
          this.onPageChange(1);
        }
        else if (this.notificationList.length > this.itemPerPage) {
          this.notificationList.shift();
        }
      },
      (error: any) => {
        console.error('Error in WebSocket connection:', error);
      },
      () => {
        console.log('WebSocket connection closed.');
      }
    );
  }
  ngOnDestroy() {
    // Unsubscribe from the WebSocket connection when the component is destroyed
    if (this.socketSubscription) {
      this.socketSubscription.unsubscribe();
    }
  }
  refreshList(){
    console.log('hi');
    this.notificationService.getNumberOfNotification().subscribe({
      next: totalList => {
        this.totalItems = totalList;
        this.totalPages = Math.floor((this.totalItems + this.itemPerPage - 1) / this.itemPerPage);

        this.notificationService.GetNotifications(this.currentPage, this.itemPerPage).subscribe({
          next: data => {
            this.notificationList = data;
            console.log((data));
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
}
