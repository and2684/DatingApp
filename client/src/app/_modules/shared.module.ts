import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    TabsModule.forRoot(), // Модуль для вкладок
    NgxGalleryModule // Просмотрщик фоточек
  ],
  exports: [
    BsDropdownModule, 
    ToastrModule,
    TabsModule, // Не забыть добавить модуль в экспорт!
    NgxGalleryModule // Просмотрщик фоточек
  ]
})
export class SharedModule { }
