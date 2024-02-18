import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {TeacherService} from "../../../_services/teacher.service";
import { AccountService } from '../../../_services/account.service';

@Component({
  selector: 'app-teacher-view',
  templateUrl: './teacher-view.component.html',
  styleUrl: './teacher-view.component.css'
})
export class TeacherViewComponent implements OnInit{

  TeacherList: any = [];
  currentUser!: string;

  currentPage: number = 1;
  itemsPerPage: number = 10;
  university: string = "all";
  department: string = "all";
  totalItems: number = 0;
  totalPages: number = 0;

  showFilter:boolean = false;
  showClearFilter:boolean = false;

  filterInfo:any = {
    uni: null,
    dep: null
  }

  constructor(private teacherService: TeacherService, private accountService: AccountService, private router: Router) {}

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
      this.teacherService.getTeachers(this.currentPage, this.itemsPerPage, this.university, this.department)
        .subscribe(data => {
          this.TeacherList = data;
        });
      //this.router.navigate(['/page', this.currentPage]);
    });
  }
  getTotalPages(): Observable<number> {
    return this.teacherService.totalTeachers(this.currentPage, this.itemsPerPage, this.university, this.department)
      .pipe(
        map(totalItems => {
          return totalItems;
        })
      );
  }
  showFilterPopup() {
    this.filterInfo.uni=this.university;
    this.filterInfo.dep=this.department;
    this.showFilter = true;
  }
  closeFilterPopup() {
    this.filterInfo.uni=null;
    this.filterInfo.dep=null;
    this.showFilter = false;
  }
  showClearFilterPopup(){
    this.showClearFilter=true;
  }
  closeClearFilterPopup(){
    this.showClearFilter=false;
  }
  clearFilter(){
    this.university="all";
    this.department="all";
    this.closeClearFilterPopup();
    this.closeFilterPopup();
    this.refreshList();
  }
  applyFilter(filterOptions: any) {
    this.university=filterOptions.university;
    this.department=filterOptions.department;
    this.refreshList();
    this.showClearFilterPopup();
    this.closeFilterPopup();
  }
  onPageChange(pageNumber: number): void {
    this.currentPage = pageNumber;
    this.refreshList();
  }
  getPageNumbers(): number[] {
    const visiblePages = 5;
    const halfVisiblePages = Math.floor(visiblePages / 2);

    let startPage = Math.max(1, this.currentPage - halfVisiblePages);
    let endPage = Math.min(startPage + visiblePages - 1, this.totalPages);

    if (endPage - startPage < visiblePages - 1) {
      startPage = Math.max(1, endPage - visiblePages + 1);
    }

    if (endPage - startPage < visiblePages - 1) {
      endPage = Math.min(this.totalPages, startPage + visiblePages - 1);
    }

    const pageNumbers: number[] = [];

    if (startPage > 1) {
      pageNumbers.push(1);
      if (startPage > 2) {
        pageNumbers.push(-1);
      }
    }

    for (let i = startPage; i <= endPage; i++) {
      pageNumbers.push(i);
    }

    if (endPage < this.totalPages) {
      if (endPage < this.totalPages - 1) {
        pageNumbers.push(-1);
      }
      pageNumbers.push(this.totalPages);
    }
    return pageNumbers;
  }
  sendMessage(friend: string){
    this.router.navigate(['/message', friend]);
  }
}
