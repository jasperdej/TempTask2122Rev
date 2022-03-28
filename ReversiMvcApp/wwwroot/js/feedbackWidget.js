class FeedbackWidget{
    constructor(elementId) {
        this._elementId = elementId;
    }

    get elementId() { //getter, set keyword voor setter methode
        return this._elementId;
    }

    show(message, type){
        let messageFixed = message.replace("\n","<br>");
        $("#feedback").text(message);
        if(type == "success"){
            $("#feedback").removeClass('alert-danger');
            $("#feedback").addClass('alert-success');
            this.log({message: 'Bijna klaar, tijd voor koffie',type: 'success'});
        }else{
            $("#feedback").removeClass('alert-success');
            $("#feedback").addClass('alert-danger');
            this.log({message: 'Niet klaar, geen tijd voor koffie',type: 'error'});
        }
        document.getElementById("feedback").style.display = "block";
    }

    show(){
        const join_element = document.querySelector('#join');

        if(join_element.classList.contains('join--hidden')){
            $("#join").removeClass('join--hidden');
        }
        $("#join").addClass('join--visible');
        join_element.style.display = "block";
    }

    hide(){
        $("#join").removeClass('join--visible');
        $("#join").addClass('join--hidden');

        const join_element = document.querySelector('#join');

        join_element.addEventListener('animationend', () => {
            if(join_element.classList.contains('join--hidden')){
                join_element.style.display = "none";
            }
        });
    }

    log(message){
        const storageObject = JSON.parse(localStorage.getItem("feedback_widget"));
        var stringJSON = '{"records":[';
        stringJSON = stringJSON.concat(JSON.stringify(message));
        if(storageObject != null){
            stringJSON = stringJSON.concat(",");
            for(var i = 0; i < Object.keys(storageObject.records).length; i++){
                if(i < 9){
                    stringJSON = stringJSON.concat(JSON.stringify(storageObject.records[i]));
                    if(i < Object.keys(storageObject.records).length - 1 && i < 8){
                        stringJSON = stringJSON.concat(",");
                    }
                }
            }
        }
        stringJSON = stringJSON.concat("]}");
        localStorage.setItem("feedback_widget", stringJSON);
    }

    removeLog(){
        localStorage.removeItem("feedback_widget");
    }

    history(){
        const storageObject = JSON.parse(localStorage.getItem("feedback_widget"));
        var returnString = '';
        if(storageObject != null){
            for(var i = 0; i < Object.keys(storageObject.records).length; i++){
                var currentRecord = storageObject.records[i];
                var text = currentRecord.type + " - " + currentRecord.message + " \n";
                returnString = returnString.concat(text);
            }
        }
        this.show(returnString, "success");
        console.log(returnString);
    }
}