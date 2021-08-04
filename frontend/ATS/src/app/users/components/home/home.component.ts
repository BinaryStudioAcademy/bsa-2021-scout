import { Component, OnInit } from '@angular/core';
import { VacancyWidgetInfo } from 'src/app/shared/models/vacancy/vacancy-widget';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit{    
  public testWidget : VacancyWidgetInfo = {    
    iconName: 'person', 
    count:58, 
    description:'Candidates', 
    list:[],
  };  

  public testWidgetFull : VacancyWidgetInfo = {
    iconName: 'business_center', 
    count:9, 
    description:'Active jobs', 
    list:[
      {isUrgent:false,name:'Developer'},
      {isUrgent:true,name:'UX Designer'},
      {isUrgent:false,name:'Office manager'},
      {isUrgent:false,name:'QA Tester'},
      {isUrgent:false,name:'Office manager'},
      {isUrgent:true,name:'UX Designer'},
    ],
  };  

  public recrutWidget : VacancyWidgetInfo = {
    iconName: 'monitor', 
    count:4, 
    description:'Recruiter', 
    list:[],
  };  

  public processedWidget : VacancyWidgetInfo = {
    iconName: 'verified_user', 
    count:12, 
    description:'Processed', 
    list:[],
  };  

  public widgets : any [] = [this.testWidget];

  ngOnInit() : void {
    console.log(this?.testWidget);
  }
}
