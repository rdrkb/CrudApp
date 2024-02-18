import {Component, OnInit} from '@angular/core';
import {AccountService} from "../../../_services/account.service";
import {Router} from "@angular/router";
import {AdminService} from "../../../_services/admin.service";
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrl: './admin-home.component.css'
})
export class AdminHomeComponent implements OnInit {
  currentUser: any;
  userData: any;

  constructor(private accountService: AccountService, private adminService: AdminService,private router: Router) {}
  ngOnInit() {
    this.currentUser = this.accountService.getCurrentUser();

    this.adminService.getAdmin(this.currentUser.username).subscribe({
      next: data => {
        this.userData = data;
      },
      error: error => {
        console.log(error);
      }
    });
  }

  partialUpdate() {
    // Add logic for partial update
    console.log('Partial update clicked');
  }

  deleteAccount() {
    // Add logic for deleting account
    console.log('Delete account clicked');
  }
}
