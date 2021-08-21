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
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
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
import { AddCandidateModalComponent }
  from './components/modal-add-candidate/modal-add-candidate.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatMenuModule } from '@angular/material/menu';
import { TagsEditComponent } from '../users/components/tags-edit/tags-edit.component';

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
    MatAutocompleteModule,
    MatCheckboxModule,
    MatChipsModule,
    MatExpansionModule,
    TopicComponent,
    FileInputComponent,
    MenuComponent,
    MatProgressBarModule,
    SpinnerComponent,
    StylePaginatorDirective,
    AddCandidateModalComponent,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatMenuModule,
    TagsEditComponent,
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
    MatAutocompleteModule,
    MatCheckboxModule,
    MatChipsModule,
    MatExpansionModule,
    BrowserAnimationsModule,
    BrowserModule,
    MatSelectModule,
    ReactiveFormsModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatAutocompleteModule,
    MatCheckboxModule,
    MatMenuModule,
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
    AddCandidateModalComponent,
    TagsEditComponent,
  ],
})
export class SharedModule { }
