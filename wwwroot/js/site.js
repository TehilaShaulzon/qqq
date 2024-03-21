const uriTasks = "https://localhost:7188/Tasks";
const uriUsers = "https://localhost:7188/Users";
//const uri = '..Tasks' אפשר גם:
let tasks = [];
userName=document.getElementById('userName')
function checkToken() {
    fetch(uriTasks, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(response => response.json())
        .then(getItems())
        .catch(error => {
            sessionStorage.setItem("check", error)
            console.log(error);
            location.href = "./login.html"

        });

}

function getItems() {
    fetch(uriTasks, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(response => response.json())
        .then(data => _displayItems(data))
        // .catch(error => console.error('Unable to get items.', error));
        .catch(error => {
            console.log(error);
            // location.href = "./login.html"

        });

}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');

    const item = {
        isDone: false,
        name: addNameTextbox.value.trim()
    };

    fetch(uriTasks, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uriTasks}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isDone: document.getElementById('edit-isDone').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uriTasks}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput('editForm');

    return false;
}

function closeInput(formToClose) {
    document.getElementById(formToClose).style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'tasks' : 'tasks kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneTaskCheckbox = document.createElement('input');
        isDoneTaskCheckbox.type = 'checkbox';
        isDoneTaskCheckbox.disabled = true;
        isDoneTaskCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneTaskCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}


if (localStorage.getItem("token") == null) {
    console.log("login");
    sessionStorage.setItem("not", "not exist token")

    location.href = "./login.html"

}
function createLink() {
    if (localStorage.getItem("link") == "true") {

        let link = document.createElement("a");
        link.href = "./userList.html";
        link.innerHTML = "users";
        console.log(sessionStorage.getItem("link"));
        document.body.appendChild(link);
    }
}
console.log(localStorage.getItem("token"));


function editUser() {

    document.getElementById('edit-name-user').value = user.name
    document.getElementById('edit-id-user').value = user.id;
    document.getElementById('edit-password-user').value = user.password;
    document.getElementById('editUserForm').style.display = 'block';
    console.log(document.getElementById('edit-id-user').value);
}
function updateUser() {
    const newUser = {
        id: user.id,
        name: document.getElementById('edit-name-user').value.trim(),
        password: document.getElementById('edit-password-user').value.trim()
    };
    fetch(`${uriUsers}/${user.id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
        body: JSON.stringify(newUser)
    })
        .then(() =>{
            user=newUser;
            userName.innerHTML=user.name;
        }
        )
        .catch(error => console.error('Unable to update item.', error));

closeInput('editUserForm')
    return false;
}
function createUser(response) {
    user = response;
    userName.innerHTML = user.name;

}
function getUser() {
    const userId = localStorage.getItem("userId");
    fetch(`${uriUsers}/${userId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(response => response.json())
        .then(response => createUser(response))
        .catch(error =>
            console.log(error));

}

let user;
getUser()
getItems();
createLink()

