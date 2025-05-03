//$(document).ready(function () {
//    loadDataTable(); // Fixed function name
//});

//function loadDataTable() {
//    $('#productTable').DataTable({
//        ajax: { url: '/admin/product/getAll' }, // Ensure this is the correct route
//        columns: [
//            { data: "title" },
//            { data: "isbn" },
//            { data: "listPrice" },
//            { data: "author" },
//            { data: "category.name" }
//        ],
//        columnDefs: [
//            { className: "text-center", targets: "_all" } // Optional styling
//        ]
//    });
//}
var DataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    DataTable = $('#productTable').DataTable({
        ajax: { url: '/admin/product/getAll' },
        columns: [
            { data: "title", width: "25%" },
            { data: "isbn", width: "15%" },
            { data: "listPrice", width: "10%" },
            { data: "author", width: "15%" },
            { data: "category.name", width: "10%" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <div class="btn-group" role="group">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-primary  mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger  mx-2">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>`;
                },
                width: "25%"
            }
        ],
        columnDefs: [
            { className: "text-center", targets: "_all" }
        ]
    });
}


function Delete (url){
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'delete',
                success: function (data) {
                    DataTable.ajax.reload();
                    toastr.success(data.message);
                }

            })
        }
    });
}