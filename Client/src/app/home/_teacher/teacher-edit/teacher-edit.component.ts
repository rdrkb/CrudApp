import {Component, OnInit} from '@angular/core';
import {AccountService} from "../../../_services/account.service";
import {TeacherService} from "../../../_services/teacher.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-teacher-edit',
  templateUrl: './teacher-edit.component.html',
  styleUrl: './teacher-edit.component.css'
})
export class TeacherEditComponent implements OnInit {
  userName: any;
  userData: any;
  formFields: any = {};
  constructor(private accountService: AccountService, private teacherService: TeacherService, private  router: Router) {}
  ngOnInit() {
    this.userName = this.accountService.getCurrentUser()['username'];
    this.teacherService.getTeacher(this.userName).subscribe({
      next: data => {
        this.userData = data;

        this.formFields["teacher_id"] = this.userData["teacher_id"];
        this.formFields["name"] = this.userData["name"];
        this.formFields["university"] = this.userData["university"];
        this.formFields["department"] = this.userData["department"];
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
      teacher_id: this.formFields['teacher_id'],
      university: this.formFields['university'],
      department: this.formFields['department'],
      gender: this.formFields['gender'],
      blood_group: this.formFields['blood_group']
    };

    this.teacherService.updateTeacher(this.userData.username, data).subscribe(
      (response) => {
        console.log('Teacher updated successfully:', response);
      },
      (error) => {
        console.error('Error updating teache:', error);
      }
    );
    this.router.navigate(['/teacherHome']);
  }

  onCancel() {
    this.router.navigate(['/teacherHome']);
  }
}
