import {Component, OnInit} from '@angular/core';
import {AccountService} from "../../../_services/account.service";
import {Router} from "@angular/router";
import {StudentService} from "../../../_services/student.service";
import {TeacherService} from "../../../_services/teacher.service";

@Component({
  selector: 'app-teacher-home',
  templateUrl: './teacher-home.component.html',
  styleUrl: './teacher-home.component.css'
})
export class TeacherHomeComponent implements OnInit {
  currentUser: any;
  userData: any;

  constructor(private accountService: AccountService, private teacherService: TeacherService,private router: Router) {}
  ngOnInit() {
    this.currentUser = this.accountService.getCurrentUser();

    this.teacherService.getTeacher(this.currentUser.username).subscribe({
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
