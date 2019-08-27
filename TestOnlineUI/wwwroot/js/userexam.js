function GetListMember(unitId) {
    $.ajax({
        url: "/Admin/TestSchedule/GetListMember?unitId=" + unitId,
        type: 'get',
        success: function (response) {
            $('#listMemberExam').html(response);
        }
    })
}
var unitId = '@listMember.First().Id';
GetListMember(unitId);

$("#selectUnit").change(function () {

    var id = this.value + '';

    GetListMember(id);
});


    $(document).ready(function () {
        $(".checkAll").on("click", function () {
            console.log(vao);
            $(this)
                .closest("table")
                .find("tbody :checkbox")
                .prop("checked", this.checked)
                .closest("tr")
                .toggleClass("selected", this.checked);
        });

        $("tbody :checkbox").on("click", function () {
            // toggle selected class to the checkbox in a row
            $(this)
                .closest("tr")
                .toggleClass("selected", this.checked);

            // add selected class on check all
            $(this).closest("table")
                .find(".checkAll")
                .prop("checked",
                    $(this)
                        .closest("table")
                        .find("tbody :checkbox:checked").length ==
                    $(this)
                        .closest("table")
                        .find("tbody :checkbox").length
                );
        });
    });

var selected = [];
var a = $('#parentcheck');
console.log(a);

