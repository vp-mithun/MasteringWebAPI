import { PacktRoutesModule } from './packt-routes/packt-routes.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { ContactsComponent } from './contacts/contacts.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ContactsComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    PacktRoutesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
