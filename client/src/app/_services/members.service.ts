import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userparams';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = []; // Тут лежат наши пользователи
  memberCache = new Map();
  user: User;
  userParams: UserParams;

  constructor(private http: HttpClient, private accountService: AccountService) {    
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
    this.user = user;
    this.userParams = new UserParams(user);
  })}

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getMembers(userParams: UserParams) {
    // console.log(Object.values(userParams).join('-')); // Мы будем использовать userParams как ключ для кэширования.
    // Посмотрели юзеров по определенным параметрам - закешировали их список и сохранили с ключом

    var response = this.memberCache.get(Object.values(userParams).join('-')); // Ищем ответ по сохраненному ключу, сгенерированному из текущих userParams
    if (response) {
      return of(response); // Если ответ с таким ключом нашелся в памяти - возвращаем его
    }

    // Иначе собираем новый ответ
    let params = this.getPaginationHeaders(
      userParams.pageNumber,
      userParams.pageSize
    );

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users/', params)
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response); // Сохраняем ответ с ключом в наш кэш (в памяти)
        return response;
      }));
  }

  getMember(username: string) {
    // const member = this.members.find((x) => x.username === username);
    // if (member !== undefined) return of(member);

    // console.log(this.memberCache) 
    
    const member = [...this.memberCache.values()] // Получаем список всех закешированных пользователей (вне зависимости от ключа)
      .reduce((arr, elem) => arr.concat(elem.result), []) // Оставляем только result - то есть готовые записи о юзерах
      .find((member: Member) => member.username === username); // И выбираем первый элемент, у которого юзернейм = юзернейму из параметров функции getMember

    if (member)
    {
      return of(member); // Если такой пользователь нашелся - вернем его из кэша (из memberCache)
    }

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

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {})
  }

  getLikes(predicate: string, pageNumber: number, pageSize: number) {
    let params = this.getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);

    return this.getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params);
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map((response) => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(
            response.headers.get('Pagination')
          );
        }
        return paginatedResult;
      })
    );
  }
}
