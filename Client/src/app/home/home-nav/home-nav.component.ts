import {Component, OnInit} from '@angular/core';
import {AccountService} from "../../_services/account.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-home-nav',
  templateUrl: './home-nav.component.html',
  styleUrl: './home-nav.component.css'
})
export class HomeNavComponent implements OnInit {
  currentUser: any;

  constructor(private accountService: AccountService, private router: Router) {}
  ngOnInit() {
    this.currentUser = this.accountService.getCurrentUser();
  }
  logout()
  {
    this.accountService.logout();
    this.router.navigate(['/']);
  }
}
