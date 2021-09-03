import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FileType } from 'src/app/shared/enums/file-type.enum';
import { CreateApplicant } from 'src/app/shared/models/applicants/create-applicant';
import { Tag } from 'src/app/shared/models/tags/tag';
import { VacancyFull } from 'src/app/shared/models/vacancy/vacancy-full';
import { ApplicantsService } from 'src/app/shared/services/applicants.service';
import { VacancyCandidateService } from 'src/app/shared/services/vacancy-candidate.service';
import { VacancyDataService } from 'src/app/shared/services/vacancy-data.service';
import { SelfApplyService } from '../../services/self-apply.service';
import { ApplyTokenInfo } from '../../models/ApplyTokenInfo';
import { environment } from 'src/environments/environment';
import { mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-self-applying',
  templateUrl: './self-applying.component.html',
  styleUrls: ['./self-applying.component.scss'],
})
export class SelfApplyingComponent {
  public allowedCvFileType = FileType.Pdf;

  public vacancyId: string = '';
  public vacancy: VacancyFull = new VacancyFull();
  public applicant: CreateApplicant = new CreateApplicant(null);
  public token: string = '';
  public email: string = '';

  public applied: boolean = false;
  public emailSend: boolean = false;
  public vacancyExist: boolean = true;
  public errorToken: boolean = false;
  public emailFilled: boolean = false;

  public validationGroup: FormGroup = new FormGroup({
    firstName: new FormControl('', [
      Validators.required,
      Validators.pattern('^[A-Z]{1}[a-z]+([\\s-]{1}[A-Z]{1}[a-z]+)?'),
    ]),
    lastName: new FormControl('', [
      Validators.required,
      Validators.pattern('^[A-Z]{1}[a-z]+([\\s-]{1}[A-Z]{1}[a-z]+)?'),
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.email,
      Validators.pattern('^\\S{1,}@\\S{3,}\\.[a-z]+'),
    ]),
    experienceDescription: new FormControl(''),
    experience: new FormControl(''),
    phone: new FormControl('', [
      Validators.required,
      Validators.pattern('^\\+?[0-9]{8,16}'),
    ]),
    linkedInUrl: new FormControl('', [
      Validators.pattern('^https:\\/\\/www.linkedin.com\\/[a-z0-9\\-]+'),
    ]),
  });

  public validationEmailGroup: FormGroup = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.email,
      Validators.pattern('^\\S{1,}@\\S{3,}\\.[a-z]+'),
    ]),
  });

  constructor(
    private readonly vacancyService: VacancyDataService,
    private readonly applicantsService: ApplicantsService,
    private readonly selfApplyService: SelfApplyService,
    private readonly vacancyCandidateService: VacancyCandidateService,
    private readonly route: ActivatedRoute) {
    this.validationEmailGroup.disable();
    this.getVacancyInfo();
  }

  public getVacancyInfo() {
    this.route.params.subscribe(({ id }) => {
      this.vacancyId = id;

      this.vacancyService.getVacancyNoAuth(id).subscribe(
        vacancy => {
          this.vacancy = vacancy;
          this.validationEmailGroup.enable();
        },
        _ => {
          this.vacancyExist = false;
        });

      this.getQueryParams();
    });
  }

  public getQueryParams() {
    this.route.queryParams
      .pipe(
        mergeMap(params => {
          this.token = params.token;
          this.email = params.email;
          return this.selfApplyService.getApplyConfirmEmailToken(params.email, this.vacancyId);
        }),
      )
      .subscribe(result => {

        if (this.token != undefined) {
          this.emailFilled = true;
          this.errorToken = true;

          result.forEach(token => {
            if (token == this.token) {
              this.errorToken = false;

              this.validationGroup.controls['email'].disable();
              this.applicant.email = this.email;

              this.getApplicant();
            }
          });
        }
        
      });
  }

  public getApplicant() {
    this.applicantsService
      .getApplicantByEmail(this.email)
      .subscribe(result => {
        if (result) {
          this.vacancyCandidateService
            .PostVacancyCandidateNoAuth(this.vacancyId, result.id)
            .subscribe(_ => {
              this.selfApplyService.MarkTokenUsed(this.token).subscribe();
              this.applied = true;
            });
        }
      });
  }

  public sendEmail() {
    this.validationEmailGroup.disable();

    let applyTokenInfo: ApplyTokenInfo = {
      email: this.validationEmailGroup.controls['email'].value,
      vacancyId: this.vacancyId,
      clientURI: `${environment.clientUrl}/vacancy/apply/${this.vacancyId}/`,
    };

    this.selfApplyService.sendApplyConfirmEmail(applyTokenInfo)
      .subscribe(_ => this.emailSend = true,
        _ => this.errorToken = true);
  }

  public updateTags(tags: Tag[]): void {
    this.applicant.tags.tagDtos = tags;
  }

  public uploadApplicantCv(files: File[]): void {
    this.applicant.cv = files[0];
  }

  public createApplicant(): void {
    this.validationGroup.disable();

    this.selfApplyService
      .addSelfAppliedApplicant(this.applicant, this.vacancyId)
      .subscribe(
        _ => {
          this.selfApplyService.MarkTokenUsed(this.token).subscribe();
          this.applied = true;
        },
      );
  }

}
