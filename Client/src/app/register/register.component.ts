import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {AccountService} from "../_services/account.service";
import {StudentService} from "../_services/student.service";
import {TeacherService} from "../_services/teacher.service";
import {AdminService} from "../_services/admin.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  @Output() cancelRegister = new EventEmitter();
  @Output() Register = new EventEmitter();
  user = { username: '', password: '', role: '' };

  constructor(private accountService: AccountService, private studentService: StudentService, private teacherService: TeacherService, private adminService: AdminService, private router: Router){}

  onSubmit(){
    this.accountService.register(this.user).subscribe({
      next: response => {
        this.accountService.saveToken(response.token)
        if(this.user.role == 'student')
        {
          this.addStudent();
          this.router.navigate(['/studentHome'])
        }
        else if(this.user.role == 'teacher')
        {
          this.addTeacher();
          this.router.navigate(['/teacherHome'])
        }
        else  if(this.user.role == 'admin')
        {
          this.addAdmin();
          this.router.navigate(['/adminHome'])
        }

      },
      error: error => console.error('Registration failed', error)
    })
    this.Register.emit();
  }
  addStudent()
  {
    var data: any = {
      username: this.user.username,
      id: "",
      student_id: "null",
      name: "null",
      university: "null",
      department: "null",
      year_of_graduation: "null",
      gender: "null",
      blood_group: "null"
    };
    console.log(data);
    this.studentService.addStudent(data).subscribe(
      (response) => {
        console.log('Student added successfully:', response);
      },
      (error) => {
        console.error('Error adding student:', error);
      });
  }
  addTeacher()
  {
    var data: any = {
      username: this.user.username,
      id: "",
      teacher_id: "null",
      name: "null",
      university: "null",
      department: "null",
      gender: "null",
      blood_group: "null"
    };
    this.teacherService.addTeacher(data).subscribe(
      (response) => {
        console.log('Teacher added successfully:', response);
      },
      (error) => {
        console.error('Error adding teacher:', error);
      });
  }
  addAdmin(){
    var data: any = {
      username: this.user.username,
      id: "",
      name: "null",
      gender: "null",
      blood_group: "null"
    };
    this.adminService.addAdmin(data).subscribe(
      (response) => {
        console.log('Admin added successfully:', response);
      },
      (error) => {
        console.error('Error adding admin:', error);
      });
  }
  onCancel(){
    this.cancelRegister.emit(false);
  }
}
