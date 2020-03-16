import { StudentService } from '@app/services';
import { Student, Sex } from '@app/models';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'app-student-create',
    templateUrl: './student-create.component.html'
})
export class StudentCreateComponent implements OnInit {
    addForm: FormGroup;
    loading = false;
    submitted = false;
    error = '';

    constructor(private formBuilder: FormBuilder,
                private studentService: StudentService,
                private router: Router,
                private toastr: ToastrService) { }

    ngOnInit() {
        this.addForm = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(40)]],
            patronymic: ['', [Validators.maxLength(60)]],
            surname: ['', [Validators.required, Validators.maxLength(40)]],
            sex: ['', Validators.required],
            studentID: ['', [Validators.maxLength(16), Validators.minLength(6)]]
        });
    }

    get form() { return this.addForm.controls; }

    onAddStudent() {
        this.submitted = true;

        if (this.addForm.invalid) {
            return;
        }

        this.loading = true;

        const student = new Student();

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

        if (this.form.studentID.value) {
            student.studentID = this.form.studentID.value;
        }
        student.surname = this.form.surname.value;

        this.studentService.addStudent(student)
            .subscribe(
                data => {
                    this.toastr.success('Студент был успешно добавлен', 'Успех');
                    this.router.navigate(['/']);
                },
                error => {
                    this.error = error;
                    this.loading = false;
                }
            );
    }
}


