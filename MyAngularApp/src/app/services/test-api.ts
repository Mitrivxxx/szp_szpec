import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TestApiService {
  private apiUrl = 'http://localhost:5032/api/User'; // Twój endpoint .NET

  constructor(private http: HttpClient) {}

  testConnection() {
    return this.http.get(this.apiUrl);
  }
}
