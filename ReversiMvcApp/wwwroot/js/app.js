class FeedbackWidget{
    constructor(elementId) {
        this._elementId = elementId;
    }

    get elementId() { //getter, set keyword voor setter methode
        return this._elementId;
    }

    show(message, type){
        let messageFixed = message.replace("\n","<br>");
        /*
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
        document.getElementById("feedback").style.display = "block";*/
    }

    show(){
        const join_element = document.querySelector('#join');
        /*
        if(join_element.classList.contains('join--hidden')){
            $("#join").removeClass('join--hidden');
        }
        $("#join").addClass('join--visible');
        join_element.style.display = "block";*/
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
const Game = (function(url){

    let configMap = {
        apiUrl : url,
    }

    let stateMap = {
        gameState:0
    }

    // Private function init
    const privateInit = function(afterInit){
        console.log(configMap.apiUrl);
        //_getCurrentGameState();
        afterInit();
    }

    const _getCurrentGameState = function(){
        setInterval(stateMap.gameState = Game.Model.getGameState(), 2000)
    }

    // Waarde/object geretourneerd aan de outer scope
    return {
        init: privateInit
    }
})('/api/url')

Game.Reversi = (function(){

    let configMap = {
        boardToken : 'null',
        spelerToken : 'null',
        spelerKleur : 'null'
    }

    // Private function init
    const privateInit = function(spToken, kleur){
        configMap.spelerToken = spToken;
        configMap.spelerKleur = kleur;
        console.log("Reversi Init");
    }

    const showFiche = function(position){
        var place = document.getElementById(position);
        place.classList.add("fiche--white");
    }

    const boardInit = async function(token){
        configMap.boardToken = token;
        var result;
        let callReturn = await Game.API.GetBoard(token);
        result = callReturn;
        console.log(result);
        var stringForHandlebars = '{"board": [';
        for(let index = 0; index < result.length; index++){
            var currentRow = result[index];
            //add row header
            var valuesToAdd = '{"row": [';
            //make grid and fiche header
            var gridValuesToAdd = '{"grid": [';
            var ficheValuesToAdd = '{"fiche": ['
            //add values
            for(let i = 0; i < 8; i++){
                //add values for grid
                gridValuesToAdd += '{"x":"' + i + '","y":"' + index + '"}';
                if(i != 7){
                    gridValuesToAdd += ',';
                }else{
                    gridValuesToAdd += ']},';
                }

                //add values for fiche
                currentFiche = currentRow[i].toLowerCase();
                ficheValuesToAdd += '{"x":"' + i + '","y":"' + index + '","ficheColour":"geen"}';
                if(i != 7){
                    ficheValuesToAdd += ',';
                }else{
                    ficheValuesToAdd += ']}';
                }
            }

            valuesToAdd += gridValuesToAdd;
            valuesToAdd += ficheValuesToAdd;
            valuesToAdd += ']}'
            if(index != 7){
                valuesToAdd += ','
            }
            stringForHandlebars += valuesToAdd;
        }
        stringForHandlebars += "]}";
        document.getElementById('reversiBoard').innerHTML = Game.Template.parseTemplate('feedbackWidget',stringForHandlebars)
        refreshBoard();
    }

    const tryDoeZet = async function(position){
        var currentAanDeBeurt = await Game.API.GetAanDeBeurt(configMap.boardToken);
        var currentAanDeBeurtFormatted = currentAanDeBeurt.toLowerCase();
        if(currentAanDeBeurtFormatted == configMap.spelerKleur){
            console.log("Aan de beurt");
            console.log(position);
            let result = await Game.API.DoeZet(configMap.boardToken, configMap.spelerToken, position);
            console.log(result);
            if(result == "Zwart" || result == "Wit" || result == "Geen"){
                await refreshBoard();
                await GameOver(result);
            }else{
                sendReversiUpdate("Zet gedaan");
                await refreshBoard();
            }
        }else{
            console.log("Niet aan de beurt");
        }
    }

    const refreshBoard = async function(){
        let bord = await Game.API.GetBoard(configMap.boardToken);
        console.log(bord);
        for(let index = 0; index < bord.length; index++){
            var currentRow = bord[index];
            for(let i = 0; i < 8; i++){
                let ficheId = index + "," + i + "fiche";
                currentFiche = currentRow[i].toLowerCase();
                intendedClass = "fiche--" + currentFiche;
                oppositeclass = "fiche--";
                if(currentFiche == "wit"){
                    oppositeclass += "zwart";
                }else if(currentFiche == "zwart"){
                    oppositeclass += "wit";
                }
                var currentFicheOnPage = document.getElementById(ficheId);
                if(currentFicheOnPage.classList.contains(intendedClass)){
                    continue;
                }else{
                    if(currentFicheOnPage.classList.contains("fiche--geen")){
                        currentFicheOnPage.classList.remove("fiche--geen");
                    }else if(currentFicheOnPage.classList.contains(oppositeclass)){
                        currentFicheOnPage.classList.remove(oppositeclass);
                    }
                    currentFicheOnPage.classList.add(intendedClass);
                }

            }
        }
        Game.Stats.addData(bord);
    }

    const geefOp = async function(){
        await Game.API.GeefOp(configMap.boardToken, configMap.spelerToken);
        GameOver();
    }

    const GameOver = async function(){
        console.log("Attempting to end");
        sendReversiUpdate("Game over");
        EndGame();
    }

    const returnToSpellen = function(){
        EndGameSecondPlayer();
    }

    // Waarde/object geretourneerd aan de outer scope
    return {
        init: privateInit,
        showFiche: tryDoeZet,
        BoardInit:boardInit,
        refreshBoard:refreshBoard,
        GeefOp:geefOp,
        GameOver:GameOver,
        returnToSpellen:returnToSpellen
    }
})()

Game.Data = (function(){

    let configMap = {
        path: 'https://peugeot9x8hype.hbo-ict.org/ReversiApi/api/Spel/',
        apiKey: 'f6a132c79543334273e89822ceb23cf4',
        mock: [
            {
                url: 'api/Spel/Beurt',
                data: 0
            }
        ]
    };

    let stateMap = {
        environment : 'production'
    }
    
    const getDecision = function(url, type){
        if(stateMap.environment == 'production'){
            return get(url, type);
        }else{
            return getMockData(url);
        }
    }

    const get = async function(url, type){
        let pathforurl = configMap.path;
        let formattedUrl = pathforurl.concat(url);
        let result = await callAPI(formattedUrl, type);
        return result;
    }

    const callAPI = async (url, type, parameter) => {
        let response = await fetch(url);
        let result;
        if(type == 'json'){
            result = await response.json(); //extract JSON from the http response
        }else if(type == 'text'){
            result = await response.text();
        }
        return result;
    }

    const put = async function(url, request){
        let pathforurl = configMap.path;
        let formattedUrl = pathforurl.concat(url);
        let response = await fetch(formattedUrl, {
            method: 'PUT',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json'
            },
            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify(request)
        });
        return await response.text();
    }

    const getMockData = function(url){
        const mockData = configMap.mock;
    
        return new Promise((resolve, reject) => {
            resolve(mockData);
        });
    
    }

    // Private function init
    const privateInit = function(env){
        if(env == 'production' || env == 'development'){
            stateMap.environment = env;
        }else{
            throw new Error("invalid environment specified");
        }
        console.log("Data Init");
    }

    // Waarde/object geretourneerd aan de outer scope
    return {
        init: privateInit,
        get:getDecision,
        put:put
    }
})()

Game.API = (function(){

    let configMap = {
        boardPath: 'bord/',
        aanDeBeurtPath: 'Beurt/',
        doeZetPath: 'Zet',
        geefOpPath: 'Opgeven'
    };

    let stateMap = {
    }

    const getData = function(url){
        return Game.Data.get(url);
    }

    const getBoard = async function(token){
        let pathforurl = configMap.boardPath;
        let formattedUrl = pathforurl.concat(token);
        return await Game.Data.get(formattedUrl, 'json');
    }

    const getAanDeBeurt = async function(token){
        let pathforurl = configMap.aanDeBeurtPath;
        let formattedUrl = pathforurl.concat(token);
        return await Game.Data.get(formattedUrl, 'text');
    }

    const doeZet = async function(token, speler, position){
        let positionX = position.charAt(0);
        let positionY = position.charAt(2);
        let positionFormatted = positionX + positionY;
        if(position == "Pas"){
            positionFormatted = position;
        }  
        let url = configMap.doeZetPath;
        const request = {
            "spelToken": token,
            "spelerToken": speler,
            "zetVeld": positionFormatted
        };
        return await Game.Data.put(url, request);
    }

    const geefOp = async function(token, speler){
        let url = configMap.geefOpPath;
        const request = {
            "spelToken": token,
            "spelerToken": speler
        };
        await Game.Data.put(url, request);
    }

    // Private function init
    const privateInit = function(){
        console.log("API Init");
    }

    // Waarde/object geretourneerd aan de outer scope
    return {
        init: privateInit,
        GetData:getData,
        GetBoard:getBoard,
        GetAanDeBeurt:getAanDeBeurt,
        DoeZet:doeZet,
        GeefOp:geefOp
    }
})()

Game.Model = (function(){

    let configMap = {
        //apiUrl : url,
    }

    const _getGameState = function(){
        Game.Data.get('/api/Spel/Beurt/').then(d => {
            if(d < 0 || d > 2){
                throw new Error('Geen valide data');
            }
            return d;
        }).catch(e =>{
            console.log(`Error opgevangen, error bericht: ${e.message}`);
        })
    }

    const getWeather = function(){
        Game.Data.get('http://api.openweathermap.org/data/2.5/weather?q=zwolle&apikey=').then(d => {
            if(!d.hasOwnProperty('temp')){
                throw new Error('Geen temperatuur beschikbaar');
            }
            console.log(d);
        }).catch(e =>{
            console.log(`Error opgevangen, error bericht: ${e.message}`);
        })
    }

    // Private function init
    const privateInit = function(){
        console.log("Model Init");
    }

    // Waarde/object geretourneerd aan de outer scope
    return {
        init: privateInit,
        getWeather: getWeather,
        getGameState: _getGameState
    }
})()

Game.Template = (function(){
    let configMap = {
        template:'test'
    }
    const privateInit = function(){
        console.log("Template Init");
    }

    const getTemplate = function(templateName){
        var template = spa_templates.templates[templateName]["body"]();
        console.log(template);
    }

    const parseTemplate = function(templateName, data){
        var template = spa_templates.templates[templateName]["body"]({boardOuter: data});
        return template
    }

    return {
        init:privateInit,
        getTemplate:getTemplate,
        parseTemplate:parseTemplate
    }
})()

Game.Stats = (function(){

    let configMap = {
        beurten : [0],
        fichesWit: [2],
        fichesZwart: [2]
    };

    // Private function init
    const privateInit = function(env){
        console.log("Stats Init");
    }

    const addData = function(bord){
        var witteFiches = 0;
        var zwarteFiches = 0;
        for(let index = 0; index < bord.length; index++){
            var currentRow = bord[index];
            for(let i = 0; i < 8; i++){
                currentFiche = currentRow[i].toLowerCase();
                if(currentFiche == "wit"){
                    witteFiches++;
                }else if(currentFiche == "zwart"){
                    zwarteFiches++;
                }
            }
        }

        configMap.fichesWit.push(witteFiches);
        configMap.fichesZwart.push(zwarteFiches);
        var beurtNumber = configMap.beurten.length + 1;
        configMap.beurten.push(beurtNumber);

        //update BalansChart
        let balans = witteFiches - zwarteFiches;
        balanceChart.data.labels.push(beurtNumber);
        balanceChart.data.datasets[0].data.push(balans);
        balanceChart.update();

        //update VeldenChart
        veldenChart.data.labels.push(beurtNumber);
        veldenChart.data.datasets[0].data.push(witteFiches);
        veldenChart.data.datasets[1].data.push(zwarteFiches)
        veldenChart.update();
    }

    // Waarde/object geretourneerd aan de outer scope
    return {
        init: privateInit,
        addData:addData
    }
})()