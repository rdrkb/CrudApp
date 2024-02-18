import {Component, OnInit} from '@angular/core';
import {AccountService} from "../_services/account.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent implements OnInit {

  model: any = {}
  loggedIn: boolean = false;
  registerMode: boolean = false;



  constructor(private accountService: AccountService, private router: Router) {}
  ngOnInit() {
    this.loggedIn = this.accountService.isAuthenticated();
    if(this.loggedIn) {
      this.move();
    }
  }
  move(){
      const currentUser=this.accountService.getCurrentUser();
      if(currentUser.role == 'admin')
        this.router.navigate(['/adminHome']);
    if(currentUser.role == 'student')
      this.router.navigate(['/studentHome']);
    if(currentUser.role == 'teacher')
      this.router.navigate(['/teacherHome']);
  }
  login()
  {
    this.accountService.login(this.model).subscribe({
      next: response => {
        this.accountService.saveToken(response.token);
        this.move();
      },
      error: error => {
        console.log(error)
      }
    });
    this.ngOnInit();
  }
  logout(){
    this.accountService.logout();
    this.ngOnInit();
  }
  onRegister(){
    this.registerMode = true;
    this.ngOnInit();
  }
  register(){
    this.registerMode = false;
    this.ngOnInit();
  }
  cancelRegisterMode(event: boolean)
  {
    this.registerMode = event;
    this.ngOnInit();
  }
}
