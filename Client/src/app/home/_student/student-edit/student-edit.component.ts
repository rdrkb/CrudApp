import {Component, OnDestroy, OnInit} from '@angular/core';
import { AccountService } from '../../../_services/account.service';
import { StudentService } from '../../../_services/student.service';
import {Router} from "@angular/router";
import {NotificationService} from "../../../_services/notification.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrl: './student-edit.component.css'
})
export class StudentEditComponent implements OnInit {
  userName: any;
  userData: any;
  formFields: any = {};

  socketSubscription!: Subscription;
  constructor(private accountService: AccountService, private studentService: StudentService,private notificationService: NotificationService, private  router: Router) {}
  ngOnInit() {
    this.userName = this.accountService.getCurrentUser()['username'];
    this.refreshPage();

    // Store the subscription
    this.socketSubscription = this.notificationService.connect('wss://localhost:7290/ws/notification').subscribe(
      (message: any) => {
        console.log('In component');
      },
      (error: any) => {
        console.error('Error in WebSocket connection:', error);
      },
      () => {
        console.log('WebSocket connection closed.');
      }
    );
  }

  refreshPage()
  {
    this.studentService.getStudent(this.userName).subscribe({
      next: data => {
        this.userData = data;

        this.formFields["student_id"] = this.userData["student_id"];
        this.formFields["name"] = this.userData["name"];
        this.formFields["university"] = this.userData["university"];
        this.formFields["department"] = this.userData["department"];
        this.formFields["year_of_graduation"] = this.userData["year_of_graduation"];
        this.formFields["gender"] = this.userData["gender"];
        this.formFields["blood_group"] = this.userData["blood_group"];
      },
      error: error =>
      {
        console.log(error);
      }
    });
  }

  onSubmit() {
    var data: any = {
      name: this.formFields['name'],
      student_id: this.formFields['student_id'],
      university: this.formFields['university'],
      department: this.formFields['department'],
      gender: this.formFields['gender'],
      year_of_graduation: this.formFields['year_of_graduation'],
      blood_group: this.formFields['blood_group']
    };

    this.studentService.updateStudent(this.userData.username, data).subscribe(
      (notification) => {
        this.notificationService.sendNotificationToWS(notification);
        //console.log('Student updated successfully:', notification);
        this.socketSubscription.unsubscribe();
      },
      (error) => {
        console.error('Error updating student:', error);
      }
    );
    this.router.navigate(['/studentHome']);
  }

  onCancel() {
    this.router.navigate(['/studentHome']);
  }
}
