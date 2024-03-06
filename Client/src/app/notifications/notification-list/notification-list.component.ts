import {Component, OnInit} from '@angular/core';
import {AdminService} from "../../_services/admin.service";
import {AccountService} from "../../_services/account.service";
import {Router} from "@angular/router";
import {NotificationService} from "../../_services/notification.service";

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent implements OnInit {

  notificationList: any = [];
  currentUser!: string;

  currentPage: number = 1;
  itemPerPage: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  constructor(private notificationService: NotificationService, private router: Router) {}

  ngOnInit() {
    this.refreshList();
  }

  refreshList(){
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
}
