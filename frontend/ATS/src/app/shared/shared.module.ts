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
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
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
import { MatTooltipModule } from '@angular/material/tooltip';
import { StylePaginatorDirective } from './directives/style-paginator.directive';
// eslint-disable-next-line
import { AddCandidateModalComponent } from './components/modal-add-candidate/modal-add-candidate.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatMenuModule } from '@angular/material/menu';
import { TagsEditComponent } from '../users/components/tags-edit/tags-edit.component';
import { DeleteConfirmComponent } from './components/delete-confirm/delete-confirm.component';
import { LogoBlockComponent } from '../users/components/logo-block/logo-block.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { UserRoleDirective } from './directives/user-role.directive';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { CopyClipboardDirective } from './directives/copy-clipboard.directive';
import { TimezonePipe } from './pipes/timezone-pipe';
import { FilesEditComponent } from '../users/components/files-edit/files-edit.component';
import { PlaceholdersEditComponent } from '../users/components/placeholders/placeholders.component';
import { AvatarModalComponent } from './components/avatar-modal/avatar-modal.component';
import { TableFilterComponent } from './components/table-filter/table-filter.component';
import { MarkBarComponent } from './components/mark-bar/mark-bar.component';
import { ConfirmationDialogComponent } 
  from './components/confirmation-dialog/confirmation-dialog.component';
import { TimezoneDateAdapter } from './date-adapters/timezone.date-adapter';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';

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
    MatGridListModule,
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
    MatTooltipModule,
    TagsEditComponent,
    DeleteConfirmComponent,
    ConfirmationDialogComponent,
    LogoBlockComponent,
    ClipboardModule,
    UserProfileComponent,
    TimezonePipe,
    FilesEditComponent,
    PlaceholdersEditComponent,
    AvatarModalComponent,
    TableFilterComponent,
    MarkBarComponent,
    NgxMaterialTimepickerModule,
    RouterModule,
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
    MatGridListModule,
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
    MatTooltipModule,
    ClipboardModule,
    NgxMaterialTimepickerModule,
  ],
  providers: [
    HttpClientService,
    { provide: DateAdapter, useClass: TimezoneDateAdapter },
  ],
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
    DeleteConfirmComponent,
    ConfirmationDialogComponent,
    LogoBlockComponent,
    UserProfileComponent,
    UserRoleDirective,
    CopyClipboardDirective,
    TimezonePipe,
    FilesEditComponent,
    PlaceholdersEditComponent,
    AvatarModalComponent,
    TableFilterComponent,
    MarkBarComponent,
  ],
})
export class SharedModule {}
