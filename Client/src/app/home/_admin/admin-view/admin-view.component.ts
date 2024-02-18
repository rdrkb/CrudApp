import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {AdminService} from "../../../_services/admin.service";
import { AccountService } from '../../../_services/account.service';

@Component({
  selector: 'app-admin-view',
  templateUrl: './admin-view.component.html',
  styleUrl: './admin-view.component.css'
})
export class AdminViewComponent implements OnInit {

  AdminList: any = [];
  currentUser!: string;

  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  constructor(private adminService: AdminService, private accountService: AccountService, private router: Router) {}
  


  ngOnInit() {
    this.currentUser = this.accountService.getCurrentUser().username;
    this.refreshList();
  }
  refreshList() {
    this.getTotalPages().subscribe(totalItems => {
      this.totalItems = totalItems;
      this.totalPages = Math.floor((this.totalItems + this.itemsPerPage - 1) / this.itemsPerPage);

      this.currentPage=Math.min(this.currentPage,this.totalPages);
      if(this.currentPage == 0)
      {
        this.currentPage=1;
      }
      this.adminService.getAdmins(this.currentPage, this.itemsPerPage)
        .subscribe(data => {
          this.AdminList = data;
        });
      //this.router.navigate(['/page', this.currentPage]);
    });
  }
  getTotalPages(): Observable<number> {
    return this.adminService.totalAdmins(this.currentPage, this.itemsPerPage)
      .pipe(
        map(totalItems => {
          return totalItems;
        })
      );
  }
  sendMessage(friend: string){
    this.router.navigate(['/message', friend]);
  }
}
