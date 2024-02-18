import {Component, OnInit} from '@angular/core';
import { AccountService } from '../../../_services/account.service';
import {Router} from "@angular/router";
import {AdminService} from "../../../_services/admin.service";
import {StudentService} from "../../../_services/student.service";

@Component({
  selector: 'app-student-home',
  templateUrl: './student-home.component.html',
  styleUrl: './student-home.component.css'
})
export class StudentHomeComponent implements OnInit{
  currentUser: any;
  userData: any;

  constructor(private accountService: AccountService, private studentService: StudentService,private router: Router) {}
  ngOnInit() {
    this.currentUser = this.accountService.getCurrentUser();

    this.studentService.getStudent(this.currentUser.username).subscribe({
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
