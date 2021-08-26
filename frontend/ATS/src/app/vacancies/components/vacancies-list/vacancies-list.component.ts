import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/users/models/user';

@Component({
  selector: 'app-vacancies-list',
  templateUrl: './vacancies-list.component.html',
  styleUrls: ['./vacancies-list.component.scss'],
})
export class VacanciesListComponent{
  user:User = {
    image: "https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?cs=srgb&dl=pexels-andrea-piacquadio-733872.jpg&fm=jpg",
    skype: "@h.roberts",
    phone:"+380123456789",
    firstName: "Hanna",
    lastName: "Roberts",
    roles: [{
      name: "HrLead",
      key: 1
    },{
      name: "HrUser",
      key: 2
    }],
    birthDate:new Date(18,1,2000),
    email:'somemail@gmail.com',
    creationDate: new Date(1,2,2021),
    isEmailConfirmed: true
  }
  constructor() { }
}
