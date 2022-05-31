import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ColdObservable } from 'rxjs/internal/testing/ColdObservable';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating app';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getUsers(); 
  }

  getUsers() {
    // Del MAS 28.05.2022
    // Old Fashioned depricated api call    
    // this.http.get('https://localhost:5001/api/users').subscribe(response => {
    //     this.users = response; 
    // }, error => { 
    //     console.log(error)
    // });
    // End Del MAS 28.05.2022

    // Add MAS 28.05.2022
    // New style api call
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (response) => {this.users = response},
      error: (error) => {console.log(error)}
    });
    // End Add MAS 28.05.2022    
  }
}
