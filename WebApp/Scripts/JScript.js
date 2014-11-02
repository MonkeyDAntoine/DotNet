$(document).ready(function(){
 /*   $('#SourceCode_TextBox').focus();
     // Force focus
     $('#SourceCode_TextBox').focusout(function(){
         $('#SourceCode_TextBox').focus();
     });*/
     
    $("#Bold_Button").on('click', function(){
        $.ajax({
            type: "POST",
            url: "Index.aspx/BoldText",
            data: '{text : "'+$('#SourceCode_TextBox').textrange('get','text')+'"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(response) {
                $('#SourceCode_TextBox').textrange('replace', response.d);
                console.log(response);
            },
            error : function(response) {
                console.log(response);
            },
            failure: function(response) {
                console.log(response);
            }
        });
    });
});