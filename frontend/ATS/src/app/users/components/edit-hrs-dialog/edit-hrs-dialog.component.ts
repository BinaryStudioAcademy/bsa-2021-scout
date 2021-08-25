import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';

@Component({
  selector: 'app-edit-hrs-dialog',
  templateUrl: './edit-hrs-dialog.component.html',
  styleUrls: ['./edit-hrs-dialog.component.scss']
})
export class EditHrsDialogComponent implements OnInit {

  constructor() { }
  hrs:User[]=[
    {
      firstName: "Emma",
    lastName: "Roberts",
    birthDate: new Date(1,1,2002),
    creationDate: new Date(1,1,2002),
    email: "aaa",
    isEmailConfirmed: true,
    roles:[
      {
        name:'HR',
        key:1
      }
    ]
    },
    {
      firstName: "Sancho",
    lastName: "Pancho",
    birthDate: new Date(1,1,2002),
    creationDate: new Date(1,1,2002),
    email: "aaa",
    isEmailConfirmed: true,
    roles:[
      {
        name:'HR',
        key:1
      }
    ]
    }
  ]

  ngOnInit(): void {
  }

}
