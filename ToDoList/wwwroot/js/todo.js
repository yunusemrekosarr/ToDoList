$('.task-input').keypress(function (e) {
    if ((e.which == 13) && ($(this).val().length > 0)) {
        var TaskInput = $(this);
        if (TaskInput.hasClass('wait')) {
            return
        }
        TaskInput.toggleClass('wait');
        $.post('/Todo/AddTodo/', { title: TaskInput.val() }).done(function (data) {
            var createdOn = moment(data.createdOn, "YYYY-MM-DDTHH:mm").format("D.MM.YYYY HH:mm:ss");

            var updatedOn = moment(data.updatedOn, "YYYY-MM-DDTHH:mm").format("D.MM.YYYY HH:mm:ss");

            var expiredOn = moment(data.expiredOn, "YYYY-MM-DDTHH:mm").format("D.MM.YYYY HH:mm:ss");

            var newTask = $(`<li class="task-list-item mb-3 list-todo" data-id=" ${data.id} " data-createdDate=" ${createdOn} " data-updatedDate=" ${updatedOn} " data-expiredDate=" ${expiredOn != 'Invalid date' ? expiredOn : ''} " data-description=" ${data.description != null ? data.description : ''} "><div class="checkbox-wrapper-39"><label><input type="checkbox" /><span class="checkbox"></span></label></div><label class="task-list-item-label"><span class="todoname-label" contenteditable="false">${TaskInput.val()}</span></label><div class="btn-container"><span class="info-btn" title="İnfo Task"></span><span class="delete-btn" title="Delete Task"></span></div>`);
            $('.task-list').prepend(newTask);
            TaskInput.val('');
            TaskInput.toggleClass('wait');
        });
    }
    else if (e.which == 13) {
        toastr.error('', 'Lütfen bir görev girin!');
    }
});

$('.submit-task').on('click', function (e) {    
    if ($('.task-input').val().length > 0) {
        var TaskInput = $('.task-input');
        if (TaskInput.hasClass('wait')) {
            return
        }
        TaskInput.toggleClass('wait');

        $.post('/Todo/AddTodo/', { title: TaskInput.val() }).done(function (data) {
            var createdOn = moment(data.createdOn, "YYYY-MM-DDTHH:mm").format("D.MM.YYYY HH:mm:ss");
            var updatedOn = moment(data.updatedOn, "YYYY-MM-DDTHH:mm").format("D.MM.YYYY HH:mm:ss");
            var expiredOn = moment(data.expiredOn, "YYYY-MM-DDTHH:mm").format("D.MM.YYYY HH:mm:ss");

            var newTask = $(`<li class="task-list-item mb-3 list-todo" data-id=" ${data.id} " data-createdDate=" ${createdOn} " data-updatedDate=" ${updatedOn} " data-expiredDate=" ${expiredOn != 'Invalid date' ? expiredOn : ''} " data-description=" ${data.description != null ? data.description : ''} "><div class="checkbox-wrapper-39"><label><input type="checkbox" /><span class="checkbox"></span></label></div><label class="task-list-item-label"><span class="todoname-label" contenteditable="false">${TaskInput.val()}</span></label><div class="btn-container"><span class="info-btn" title="İnfo Task"></span><span class="delete-btn" title="Delete Task"></span></div>`);
            $('.task-list').prepend(newTask);
            TaskInput.val('');
            TaskInput.toggleClass('wait');
        });
    } else {
        toastr.error('' ,'Lütfen bir görev girin!');
    }
});

$('.task-list').on('click', '.info-btn', function () {
    var a = $(this).closest(".list-todo");
    var theTodo = a.find(".todoname-label").text().trim();
 
    $('.details').attr('data-id', a.data('id'));
    $('.details .todoname').text(theTodo);

    var updatedDatde = moment(a.attr('data-updatedDate'), "D.MM.YYYY HH:mm:ss", "tr").format("YYYY-MM-DDTHH:mm");
    $('.details #updated').val(updatedDatde);

    var expiredDatde = moment(a.attr('data-expiredDate').trim(), "D.MM.YYYY HH:mm:ss", "tr").format("YYYY-MM-DDTHH:mm");
    $('.details #expired').val(expiredDatde);

    var description = a.attr('data-description');
 
    if (description.trim() !== "")
        $('.add-desc').val(description);
    else
        $('.add-desc').val('');

    $('.details').removeClass('none').addClass('col-md-4');
    $('.main').removeClass('col-md-12').addClass('col-md-8');
});

$('.task-list').on('click', '.checkbox', function () {
    var todoLi = $(this).closest(".task-list-item");    
    if (todoLi.hasClass('wait')) {
        var checkbox = $(this).siblings('input[type="checkbox"]');
        if (todoLi.find('.task-list-item-label').hasClass('done'))
            checkbox.prop('checked', true); 
        else
            checkbox.prop('checked', false);         
        return
    }
    todoLi.addClass('wait');
    $.post('/Todo/ToggleTodo/', { Id: todoLi.data('id') }).done(function () {
        todoLi.find('.task-list-item-label').toggleClass('done');
        todoLi.removeClass('wait');
    });
});

$('.task-list').on('click', '.delete-btn', function () {
    var todoLi = $(this).closest(".task-list-item");
    $.post('/Todo/DeleteTodo/', { Id: todoLi.data('id') }).done(function () {
        todoLi.remove();
    })
    $('.btn-close-white').trigger('click');
})

$('.todonameparent').on('dblclick', '.todoname', function () {
    $(this).attr('contenteditable', 'true').focus();
});

$('.todonameparent').on('blur', '.todoname', function () {
    $(this).attr('contenteditable', 'false');
});

$('.update-task').on('click', function () {
    $('.update-img').addClass('animate__animated  animate__rotateIn --animate-duration: 800ms');
    var updatedId = $(".details").attr('data-id');
    var title = $(".todoname").text();
    var expiredDate = $("#expired").val();
    var updatedDate = $("#updated").val();
    var desc = $(".add-desc").val();
    var updated =  moment().format('D.MM.YYYY HH:mm:ss');
    var updatedformat = moment(updated, "D.MM.YYYY HH:mm:ss").format("YYYY-MM-DDTHH:mm");
    var expired = moment(expiredDate, "YYYY-MM-DDTHH:mm").format("D.MM.YYYY HH:mm:ss");
    $.post('/Todo/UpdateTodo/', { Id: updatedId, Title: title, ExpiredOn: expiredDate, UpdatedOn: updatedformat, Description: desc }).done(function () {
        var updatedLi = $(`.list-todo[data-id="${updatedId}"]`);
        var createdDate = updatedLi.data('createdDate');
        updatedLi.attr('class', 'task-list-item mb-3 list-todo');
        updatedLi.attr('data-id', updatedId);
        updatedLi.attr('data-createdDate', createdDate);
        updatedLi.attr('data-updatedDate', updated);
        updatedLi.attr('data-expiredDate', expired);
        updatedLi.attr('data-description', desc);
        updatedLi.find('.todoname-label').text(title);
        $('.details #updated').val(updatedformat);
    setTimeout(function () {
        $('.update-img').removeClass('animate__animated animate__rotateIn --animate-duration: 800ms');
    }, 800);
    })
});



