import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OnInit } from '@angular/core';
import { TestApiService } from './services/test-api';
import { JsonPipe, CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  template: `
    <h1>Test API</h1>
    <button (click)="checkBackend()">Sprawdź połączenie</button>
    <p *ngIf="response">{{ response | json }}</p>
    <p *ngIf="error" style="color:red;">{{ error }}</p>
  `,
  styleUrls: ['./app.scss']
})
export class App implements OnInit {
  response: any;
  error: any;

  constructor(private testApi: TestApiService) {}

  ngOnInit() {}

  checkBackend() {
    this.testApi.testConnection().subscribe({
      next: res => this.response = res,
      error: err => this.error = 'Błąd połączenia: ' + err.message
    });
  }
}