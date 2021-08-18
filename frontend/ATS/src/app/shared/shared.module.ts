import { NgModule } from '@angular/core';
import { HttpClientService } from './services/http-client.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FormsModule } from '@angular/forms';
import { ButtonComponent } from './components/button/button.component';
import { HeaderComponent } from './components/header/header.component';
import { SearchFormComponent } from './components/search-form/search-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MultiselectComponent } from './components/multiselect/multiselect.component';
import { TopicComponent } from './components/topic/topic.component';
import { FileInputComponent } from './components/file-input/file-input.component';
import { MenuComponent } from './components/menu/menu.component';
import { MainContentComponent } from './components/main-content/main-content.component';
import { RouterModule } from '@angular/router';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { MatDialogModule } from '@angular/material/dialog';
import { StylePaginatorDirective } from './directives/style-paginator.directive';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';



@NgModule({
  exports: [
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatBadgeModule,
    MatSidenavModule,
    MatProgressSpinnerModule,
    FormsModule,
    ButtonComponent,
    SearchFormComponent,
    ReactiveFormsModule,
    CommonModule,
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    HeaderComponent,
    MatListModule,
    MatSelectModule,
    MultiselectComponent,
    MatDatepickerModule,
    MatDialogModule,
    MatNativeDateModule,
    TopicComponent,
    FileInputComponent,
    MenuComponent,
    MatProgressBarModule,
    SpinnerComponent,
    StylePaginatorDirective,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatChipsModule,
    MatAutocompleteModule,
  ],
  imports: [
    MatButtonModule,
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatBadgeModule,
    MatSidenavModule,
    MatProgressSpinnerModule,
    FormsModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatProgressBarModule,
    MatListModule,
    BrowserAnimationsModule,
    BrowserModule,
    MatSelectModule,
    ReactiveFormsModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatChipsModule,
    MatAutocompleteModule,
  ],
  providers: [HttpClientService],
  declarations: [
    MultiselectComponent,
    ButtonComponent,
    SearchFormComponent,
    HeaderComponent,
    TopicComponent,
    FileInputComponent,
    MenuComponent,
    MainContentComponent,
    SpinnerComponent,
    StylePaginatorDirective,
  ],
})
export class SharedModule {}