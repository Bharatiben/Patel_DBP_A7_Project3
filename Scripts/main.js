var Sortable = {
    baseUrl: '',
    sortBy: 0,
    searchTerm: '',

    Search() {
        var searchKey = $('#txtSearch').val();
        window.location.href = Sortable.baseUrl + searchKey;
    },

    Sort(sortBy) {
        var isDesc = true;
        const urlParams = new URLSearchParams(window.location.search);
        var isDescOriginal = urlParams.get('isDesc');
        const sortByOriginal = urlParams.get('sortBy');

        //sorting the same column, means we need to change the isDesc value
        if (sortByOriginal == sortBy) {
            if (isDescOriginal == 'true') {
                isDesc = false;
            }
        }
        window.location.href = Sortable.baseUrl + "?sortBy=" + sortBy + "&isDesc=" + isDesc;
    }
};

var apiHandler = {
    GET(url) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                debugger;
            }
        });
    },
    POST(url, object) {
        object = {
            Id: 5,
            Name: "asd",
            //....
        }

        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                debugger;
            }
        });
    },
    DELETE(url) {
        if (confirm("Are u sure you want to delete??")) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (res) {
                    //we can redirect
                    //we can show msg
                    // we can throw a error
                    //debugger;

                    //new for delete
                    if (res.Success == true) {
                        debugger;
                        //location.href = res.baseUrl;
                        location.href = res.returnUrl;
                    } else {
                        alert(res.Message);
                    }
                }
            });
        } else {
            alert("Delete cancelled...");
        }

    }

};