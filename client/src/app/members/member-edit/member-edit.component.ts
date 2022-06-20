import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { Observable, take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm; // Получаем доступ к нашей форме editForm из компонента (файла .ts)
  member: Member;
  user: User;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  } // Host listener позволяет получать доступ к eventам браузера. Здесь мы проверяем, что содержимое формы поменялось (form.dirty=true)

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(currentuser => this.user = currentuser); // Получим текущего пользователя в локальную переменную класса (user: User)
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => this.member = member);
  }

  updateMember() { // Сохранить изменения
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success("Profile updated successfully!");
      this.editForm.reset(this.member); // Перезагрузить форму, сохранив текущего пользователя 
    });
  }


}
