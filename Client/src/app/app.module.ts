import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HttpClientModule} from "@angular/common/http";
import { FormsModule} from "@angular/forms";
import { RegisterComponent } from './register/register.component';
import { TeacherViewComponent } from './home/_teacher/teacher-view/teacher-view.component';
import { StudentViewComponent } from './home/_student/student-view/student-view.component';
import { HomeNavComponent } from './home/home-nav/home-nav.component';
import { AdminViewComponent } from './home/_admin/admin-view/admin-view.component';
import { AdminEditComponent } from './home/_admin/admin-edit/admin-edit.component';
import { StudentEditComponent } from './home/_student/student-edit/student-edit.component';
import { TeacherEditComponent } from './home/_teacher/teacher-edit/teacher-edit.component';
import { AdminHomeComponent } from './home/_admin/admin-home/admin-home.component';
import { TeacherHomeComponent } from './home/_teacher/teacher-home/teacher-home.component';
import { StudentHomeComponent } from './home/_student/student-home/student-home.component';
import { AdminPartialUpdateComponent } from './home/_admin/admin-partial-update/admin-partial-update.component';
import { StudentPartialUpdateComponent } from './home/_student/student-partial-update/student-partial-update.component';
import { TeacherPartialUpdateComponent } from './home/_teacher/teacher-partial-update/teacher-partial-update.component';
import { StudentFilterComponent } from './home/_student/student-filter/student-filter.component';
import { TeacherFilterComponent } from './home/_teacher/teacher-filter/teacher-filter.component';
import { MessageListComponent } from './messages/message-list/message-list.component';
import { MessageComponent } from './messages/message/message.component';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterComponent,
    TeacherViewComponent,
    StudentViewComponent,
    HomeNavComponent,
    AdminViewComponent,
    AdminEditComponent,
    StudentEditComponent,
    TeacherEditComponent,
    AdminHomeComponent,
    TeacherHomeComponent,
    StudentHomeComponent,
    AdminPartialUpdateComponent,
    StudentPartialUpdateComponent,
    TeacherPartialUpdateComponent,
    StudentFilterComponent,
    TeacherFilterComponent,
    MessageListComponent,
    MessageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    provideClientHydration()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
