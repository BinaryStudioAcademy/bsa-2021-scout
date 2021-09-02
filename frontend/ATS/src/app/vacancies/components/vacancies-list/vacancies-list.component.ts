import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/users/models/user';

@Component({
  selector: 'app-vacancies-list',
  templateUrl: './vacancies-list.component.html',
  styleUrls: ['./vacancies-list.component.scss'],
})
export class VacanciesListComponent{
  constructor() { }
  user:User={
    firstName: "Emma",
  lastName: "Roberts",
  birthDate: new Date(1,1,2002),
  creationDate: new Date(1,1,2002),
  email: "emma.roberts@gmail.com",
  phone:"+38077456626",
  skype:"@emmam",
  isEmailConfirmed: true,
  roles:[
    {
      name:'HrUser',
      key:1
    }
  ],
  image:"https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?cs=srgb&dl=pexels-andrea-piacquadio-733872.jpg&fm=jpg"
  }
}
