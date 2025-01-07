var UpdatedRow;
function ShowSuccessMessage(message = 'Saved Successfully') {
    Swal.fire({
        title: "Done",
        text: message,
        icon: "success"
    });
} function ShowErrorMessage(message = 'Something went wrong') {
    Swal.fire({
        title: "Oooops!",
        text: message,
        icon: "error",
    });
}
//function onModalBegin() {
//    $('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');
//}

function OnModelSuccess(item) {
    ShowSuccessMessage();
    $('#Modal').modal('hide');
    if (UpdatedRow === undefined) {
        $('tbody').append(item);
    }        
    else 
    {
        $(UpdatedRow).replaceWith(item);
        UpdatedRow = undefined;
    }
}

//function onModalComplete() {
//    $('body :submit').removeAttr('disabled').removeAttr('data-kt-indicator');
//}

$(document).ready(function () {

    //select2
    $('.js-selsect2').select2();

    //Datepicker//use text type for better experinace
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        drops: 'up',
        maxDate: new Date()
    });
    //TinyMCE
    tinymce.init({
        selector: ".js-tinymce", height: "422",
        plugins: [
            // Core editing features
            'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'link', 'lists', 'media', 'searchreplace', 'table', 'visualblocks', 'wordcount',
            // Your account includes a free trial of TinyMCE premium features
            // Try the most popular premium features until Jan 19, 2025:
            'checklist', 'mediaembed', 'casechange', 'export', 'formatpainter', 'pageembed', 'a11ychecker', 'tinymcespellchecker', 'permanentpen', 'powerpaste', 'advtable', 'advcode', 'editimage', 'advtemplate', 'ai', 'mentions', 'tinycomments', 'tableofcontents', 'footnotes', 'mergetags', 'autocorrect', 'typography', 'inlinecss', 'markdown', 'importword', 'exportword', 'exportpdf'
        ],
        toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
        tinycomments_mode: 'embedded',
        tinycomments_author: 'Author name',
        mergetags_list: [
            { value: 'First.Name', title: 'First Name' },
            { value: 'Email', title: 'Email' },
        ],
        ai_request: (request, respondWith) => respondWith.string(() => Promise.reject('See docs to implement AI Assistant')),
    });

    var message = $('#Message').text();
    if (message != '') ShowSuccessMessage(message);

    //Handle bootstrap modal
    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');

        modal.find('#ModalLabel').text(btn.data('title'));

        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr');
        }

        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                showErrorMessage();
            }
        });

        modal.modal('show');
    });
    //Handle Toggle Status
    $(document).ready(function () {
        $('table').DataTable();

        $('body').delegate('.js-toggle-status', 'click', function () {
            var btn = $(this);

            if (confirm("Are you sure you wont to toggle it ?")) {
                $.post({
                    url: btn.data('url'),
                    data: {
                        '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                    },
                    success: function (LastUpdatedOn) {
                        var row = btn.parents('tr');
                        var status = row.find('.js-status');
                        var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                        status.text(newStatus).toggleClass('badge-success badge-danger');
                        row.find('.js-updated-on').html(LastUpdatedOn);
                        row.addClass('animate__animated animate__flash');
                        ShowSuccessMessage();
                    },
                    error: function () {
                        ShowErrorMessage();
                    }
                });
            }
        });
    });
});
