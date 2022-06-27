import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    TabsModule.forRoot(), // Модуль для вкладок
    NgxGalleryModule, // Просмотрщик фоточек
    NgxSpinnerModule, // Крутяшка для визуализации загрузки
    FileUploadModule // Загрузчик файлов (Картинок)
  ],
  exports: [
    BsDropdownModule, 
    ToastrModule,
    TabsModule, // Не забыть добавить модуль в экспорт!
    NgxGalleryModule, // Просмотрщик фоточек
    NgxSpinnerModule, // Крутяшка для визуализации загрузки
    FileUploadModule // Загрузчик файлов (Картинок)
  ]
})
export class SharedModule { }
