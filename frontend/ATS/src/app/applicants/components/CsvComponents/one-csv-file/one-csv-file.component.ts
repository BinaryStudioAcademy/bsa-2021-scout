import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CsvFile } from '../../../models/CsvFile';

@Component({
  selector: 'app-one-csv-file',
  templateUrl: './one-csv-file.component.html',
  styleUrls: ['./one-csv-file.component.scss'],
})
export class CsvFileComponent {
  @Input() csvFile: CsvFile = new CsvFile();

  constructor(private router: Router) { }

  public moveToTablesPage() {
    window.localStorage.setItem('csvFile', JSON.stringify(this.csvFile));
    this.router.navigate(['applicants/csv']);
  }
}
