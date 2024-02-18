import {Component, OnInit} from '@angular/core';
import { AccountService } from '../../../_services/account.service'; 
import {AdminService} from "../../../_services/admin.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-admin-edit',
  templateUrl: './admin-edit.component.html',
  styleUrl: './admin-edit.component.css'
})
export class AdminEditComponent implements OnInit {

  userName: any;
  userData: any;
  formFields: any = {};
  constructor(private accountService: AccountService, private adminService: AdminService, private  router: Router) {}
  ngOnInit() {
    this.userName = this.accountService.getCurrentUser()['username'];
    this.adminService.getAdmin(this.userName).subscribe({
      next: data => {
        this.userData = data;

        this.formFields["name"] = this.userData["name"];
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
      gender: this.formFields['gender'],
      blood_group: this.formFields['blood_group']
    };

    this.adminService.updateAdmin(this.userData.username, data).subscribe(
      (response) => {
        console.log('Admin updated successfully:', response);
      },
      (error) => {
        console.error('Error updating admin:', error);
      }
    );
    this.router.navigate(['/adminHome']);
  }

  onCancel() {
    this.router.navigate(['/adminHome']);
  }
}
