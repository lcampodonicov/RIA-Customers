
$("#BtnGenerate").on("click", async () => {

    var $Button = $("#BtnGenerate");
    $Button.addClass("disabled");

    ShowInfo("Sending...");

    try {

        var $Res = await PostGeneratedCustomers(10);

        if( $Res.Success ){
            
            ShowSuccess("Customers sent correctly");

            await ShowNewCustomers();
            ShowReset();

        } else {
            ShowErrors( $Res.Message, $Res.Errors );
        }

    } catch (e) {
        ShowErrors("Ocurrió un error");
    }

    $Button.removeClass("disabled");

});

$("#BtnReset").on("click", ResetDatabase);

function ShowReset(){

    $("#BtnReset").css({ display: "inline-block" });

}

function HideReset(){

    $("#BtnReset").css({ display: "none" });

}

async function ResetDatabase(){

    var $Res = await $.ajax({
        type: "DELETE",
        url: "/Customer"
    });

    if( $Res.Success ){
        ShowInfo( "Database successfully reset" );
        $("#NewCustomers").html("");
        HideReset();
    }

}

async function ShowNewCustomers(){

    var $TableHeaders = ["Id", "First Name", "Last Name", "Age"];

    var $Res = await $.ajax({
        type: "GET",
        url:  "/Customer"
    });

    if( $Res.Success )
        var $NewCustomers = $Res.Customers;

    $("#NewCustomers")
        .append(
            $("<table>")
                .addClass("table table-striped table-bordered")
                .append(
                    $("<thead>").append(
                        $("<tr></tr>")
                        .append(
                            $TableHeaders
                                .map(
                                    $TableHeader =>
                                        $("<th></th>")
                                            .text( $TableHeader )
                                )
                        )
                    )
                )
                .append(
                    $NewCustomers
                        .map(
                            $$NewCustomer =>
                                $("<tbody>")
                                    .append(
                                        $("<tr></tr>").append(
                                            Object.values(
                                                $$NewCustomer
                                            ).map(
                                                $Value =>
                                                    $("<td></td>")
                                                        .text( $Value )
                                            )
                                        )
                                    )
                        )
                )
        )
    ;

}

function ShowInfo( $Text ){
    var $Status = $("#Status");
    $Status.css({ color: "black" });
    $Status.text( $Text );
}

function ShowSuccess( $Text ){
    var $Status = $("#Status");
    $Status.css({ color: "green" });
    $Status.text( $Text );
}

function ShowErrors( $Text, $Errors ){

    var $Status = $("#Status");
    $Status.css({ color: "red" });

    $Errors = $Errors || [];

    $Status
        .html( "" )
        .append( $("<text></text>").text( $Text ) )
        .append(
            $("<ul></ul>")
                .addClass("list-unstyled")
                .append(
                    $Errors
                        .map(
                            $Error =>
                                $("<li></li>").text( $Error )
                        )
                )
        )
    ;

}

var FirstNames = [
    "Leia",
    "Sadie",
    "Jose",
    "Sara",
    "Frank",
    "Dewey",
    "Tomas",
    "Joel",
    "Lukas",
    "Carlos",
];

var LastNames = [
    "Liberty",
    "Ray",
    "Harrison",
    "Ronan",
    "Drew",
    "Powell",
    "Larsen",
    "Chan",
    "Anderson",
    "Lane",
];

var GetRandomElement = ($List) =>
    $List[GetRandom(0, $List.length - 1)]
;

var GetRandom = (Min, Max) =>
    Math.floor( Math.random() * (Max - Min + 1) ) + Min;
;

var GenerateCustomers = ($Amount) =>
    [...Array($Amount)]
        .map(
            (_, $Index) => ({
                Id: $Index,
                FirstName: GetRandomElement(FirstNames),
                LastName: GetRandomElement(LastNames),
                Age: GetRandom(10, 90)
            })
        )
;

var PostGeneratedCustomers = async ($Amount) => {

    var $Customers = GenerateCustomers( 10 );

    return $.ajax({
        data: {
            Customers: $Customers
        },
        type: "POST",
        url: "/Customer"
    });

}
