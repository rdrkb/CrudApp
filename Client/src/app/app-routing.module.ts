import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { StudentViewComponent } from "./home/_student/student-view/student-view.component";
import { TeacherViewComponent } from "./home/_teacher/teacher-view/teacher-view.component";
import { AdminViewComponent } from "./home/_admin/admin-view/admin-view.component";
import { AdminEditComponent } from "./home/_admin/admin-edit/admin-edit.component";
import { StudentEditComponent } from "./home/_student/student-edit/student-edit.component";
import { TeacherEditComponent } from "./home/_teacher/teacher-edit/teacher-edit.component";
import { AdminHomeComponent } from "./home/_admin/admin-home/admin-home.component";
import { StudentHomeComponent } from "./home/_student/student-home/student-home.component";
import { TeacherHomeComponent } from "./home/_teacher/teacher-home/teacher-home.component";
import {MessageListComponent} from "./messages/message-list/message-list.component";
import {MessageComponent} from "./messages/message/message.component";

const routes: Routes = [
  { path: '', component: NavComponent },
  { path: 'studentView', component: StudentViewComponent },
  { path: 'teacherView', component: TeacherViewComponent },
  { path: 'adminView', component: AdminViewComponent },
  { path: 'adminEdit', component: AdminEditComponent },
  { path: 'studentEdit', component: StudentEditComponent },
  { path: 'teacherEdit', component: TeacherEditComponent },
  { path: 'adminHome', component: AdminHomeComponent },
  { path: 'studentHome', component: StudentHomeComponent },
  { path: 'teacherHome', component: TeacherHomeComponent },
  { path: 'chatList', component: MessageListComponent },
  { path: 'message/:username', component: MessageComponent }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
