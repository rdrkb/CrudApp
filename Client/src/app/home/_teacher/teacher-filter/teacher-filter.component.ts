import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-teacher-filter',
  templateUrl: './teacher-filter.component.html',
  styleUrl: './teacher-filter.component.css'
})
export class TeacherFilterComponent implements OnInit{

  @Input() filterInfo:any = {
    uni: null,
    dep: null
  }

  @Output() close = new EventEmitter<void>();
  @Output() applyFilter = new EventEmitter<any>();

  filterOptions = {
    university: null,
    department: null
  };

  universities = [
    "all",
    "Shahjalal University of Science and Technology (SUST)",
    "University of Dhaka",
    "Bangladesh University of Engineering and Technology (BUET)",
    "Jahangirnagar University",
    "Khulna University",
    "Rajshahi University",
    "Chittagong University",
    "Islamic University, Bangladesh",
    "Comilla University",
  ]
  departments = [
    "all",
    "Computer Science and Engineering",
    "Electrical and Electronic Engineering",
    "Mechanical Engineering",
    "Physics",
    "Chemistry",
    "Mathematics"
  ]
ngOnInit() {
    this.filterOptions.university=this.filterInfo.uni;
    this.filterOptions.department=this.filterInfo.dep;
}

  applyfilter() {
    this.applyFilter.emit(this.filterOptions);
  }
  closefilter(){
    this.close.emit();
  }
}
