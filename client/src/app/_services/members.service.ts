import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = []; // Тут лежат наши пользователи

  constructor(private http: HttpClient) {}

  getMembers(){
    if (this.members.length > 0)
      return of(this.members); // Если в массиве есть пользователи, вернем его и не будем ничего перезагружать от АПИ. Of используется чтобы вернуть members as observable и к этому методу можно было подписаться
    else
      return this.http.get<Member[]>(this.baseUrl + 'users').pipe( // Если в members пусто, то запрашиваем АПИ
        map(members => { // map тоже возвращает observable
          this.members = members;          
          return members;
        })        
      );
  }

  getMember(username: string){
    const member = this.members.find(x => x.username === username);

    if (member !== undefined) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) { 
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        // т.к. HttpPut не возвращает значений, то мы после апдейта будем искать пользователя в members
        const i = this.members.indexOf(member); // Получаем индекс элемента массива для нашего пользователя (i)
        this.members[i] = member; // Обновляем запись в массиве пользователей
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId, {});
  }
}
