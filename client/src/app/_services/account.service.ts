import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { User } from '../_models/user';

//los servicios de angular son singleton, no se destruyen hasta q se cierra la app
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();
 
  constructor(private http: HttpClient) { }
 
  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map(response => {
        const user = response as User;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  } 
 
  register(model: any){
    return this.http.post(this.baseUrl + 'accout/register', model).pipe(
      map((user : User) => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }
 
  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
