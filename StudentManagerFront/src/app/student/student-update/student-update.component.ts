import { StudentService } from '@app/services';
import { Student, Sex } from '@app/models';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-student-update',
    templateUrl: './student-update.component.html'
})
export class StudentUpdateComponent implements OnInit {
    updateForm: FormGroup;
    loading = false;
    submitted = false;
    error = '';
    id: string;

    constructor(private formBuilder: FormBuilder,
                private route: ActivatedRoute,
                private router: Router,
                private studentService: StudentService,
                private toastr: ToastrService) { }

    ngOnInit() {
        this.updateForm = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(40)]],
            patronymic: ['', [Validators.maxLength(60)]],
            surname: ['', [Validators.required, Validators.maxLength(40)]],
            sex: ['', Validators.required],
            studentID: ['', [Validators.maxLength(16), Validators.minLength(6)]]
        });

        this.id = this.route.snapshot.params.id;

        this.studentService.getStudent(this.id)
            .subscribe(
                data => {
                    this.updateForm.controls.name.setValue(data.name);
                    this.updateForm.controls.patronymic.setValue(data.patronymic);
                    this.updateForm.controls.surname.setValue(data.surname);
                    switch (data.sex) {
                        case Sex.Man:
                            this.updateForm.controls.sex.setValue('Мужской');
                            break;
                        case Sex.Women:
                            this.updateForm.controls.sex.setValue('Женский');
                            break;
                        default:
                            this.updateForm.controls.sex.setValue('Женский');
                    }
                    this.updateForm.controls.studentID.setValue(data.studentID);
                },
                error => {
                    this.toastr.error('Не удалось открыть форму редактирования студента', 'Ошибка');
                    this.router.navigate(['/']);
                }
            );
    }

    get form() { return this.updateForm.controls; }

    onUpdateStudent() {
        this.submitted = true;

        if (this.updateForm.invalid) {
            return;
        }

        this.loading = true;

        const student = new Student();

        student.id = this.id;
        student.name = this.form.name.value;
        student.patronymic = this.form.patronymic.value;
        switch (this.form.sex.value) {
            case 'Мужской':
                student.sex = Sex.Man;
                break;
            case 'Женский':
                student.sex = Sex.Women;
                break;
            default:
                student.sex = Sex.Man;
        }
        student.studentID = this.form.studentID.value;
        student.surname = this.form.surname.value;


        this.studentService.updateStudent(this.id, student)
            .subscribe(
                data => {
                    this.toastr.success('Студент был успешно отредактирован', 'Успех');
                    this.router.navigate(['/']);
                },
                error => {
                    this.error = error;
                    this.loading = false;
                }
            );
    }
}
