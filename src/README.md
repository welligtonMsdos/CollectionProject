# 💿 Collection Management API

> This API manages the CRUD operations for music collections, specifically focusing on **Vinyl Records** and **Live Shows**.

## 🛠 Technologies & Libraries

<div style="display: flex; gap: 5px; flex-wrap: wrap;">
  <img src="https://img.shields.io/badge/.NET%2010-5C2D91?style=for-the-badge&logo=.net&logoColor=white"/>
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white"/>
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=PostgreSQL&logoColor=white"/>
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=Docker&logoColor=white"/>
</div>

### Backend Ecosystem
* **AutoMapper** – Effortless object-to-object mapping between Entities and DTOs.
* **FluentValidation** – Strongly-typed validation rules for clean and reliable data input.
* **Entity Framework Core & Dapper** – Hybrid approach for optimized write and read database operations.

---

<details> 
  
<summary><b>Vinyl Records Endpoints (Click to expand)</b></summary>

<br>

<details> 
  
<summary>Create Vinyl</summary>

<br>

Adds a new vinyl to the collection

    Request: POST /api/Vinil/CreateVinil

Body:
     
    {
     "artist": "Guns N' Roses",
     "album": "Use Your Illusion I",
     "year": 1991,
     "photo": "[https://example.com/cover.jpg](https://example.com/cover.jpg)",
     "price": 50.00
    }

Response:

    {
      "success": true,
      "data": {
      "id": 13,
      "artist": "Guns N' Roses",
      "album": "Use Your Illusion I",
      "year": 1991,
      "photo": "[https://example.com/cover.jpg](https://example.com/cover.jpg)",
      "price": 50.00
    },
    "message": "Vinil criado com sucesso!",
    "errors": null
    }

</details> 

<details> 
  
<summary>Get All Vinyl Records</summary>

<br>

Returns a list of all vinyl records in the collection.

Request: GET /api/Vinil/GetAllVinis
       
       curl -i -H 'Accept: application/json' http://localhost:5012/api/Vinil/GetAllVinis

Response:

     {
       "success": true,
       "data": [
        {
          "id": 1,
          "artist": "Guns N' Roses",
          "album": "Appetite for Destruction",
          "year": 1987,
          "photo": "[https://link-to-photo.com/image.jpg](https://link-to-photo.com/image.jpg)",
          "price": 45.90
        }
       ],
       "message": "Operation completed successfully",
       "errors": null
    }

</details>

<details> 
  
<summary>Get Vinyl Records by id</summary>

<br>

Returns a list of vinyl records in the collection by id.

Request: GET /api/Vinil/GetVinilById/{id}
       
       curl -i -H 'Accept: application/json' http://localhost:5012/api/Vinil/GetVinilById/2

Response: 

    {
      "success": true,
      "data": {
      "id": 2,
      "artist": "Pink Floyd",
      "album": "The Dark Side of the Moon",
      "year": 1973,
      "photo": "https://tse2.mm.bing.net/th/id/OIP.FM0xRE9_gOlpBe-qMSs9PAHaGq?rs=1&pid=ImgDetMain&o=7&rm=3",
      "price": 1
    },
    "message": "Operação realizada com sucesso",
    "errors": null  
    }

</details>

<details> 

<summary>Update Vinyl to the collection</summary>

Request: GET /api/Vinil/UpdateVinil

Body:
     
    {
      "id": 2,
      "artist": "Pink Floyd",
      "album": "The Dark Side of the Moon",
      "year": 1973,
      "photo": "https://tse2.mm.bing.net/th/id/OIP.FM0xRE9_gOlpBe-qMSs9PAHaGq?rs=1&pid=ImgDetMain&o=7&rm=3",
      "price": 100
    }

Response:

    {
      "success": true,
      "data": {
      "id": 2,
      "artist": "Pink Floyd",
      "album": "The Dark Side of the Moon",
      "year": 1973,
      "photo": "https://tse2.mm.bing.net/th/id/OIP.FM0xRE9_gOlpBe-qMSs9PAHaGq?rs=1&pid=ImgDetMain&o=7&rm=3",
      "price": 100
     },
     "message": "Vinil atualizado com sucesso!",
     "errors": null
    }

</details>

<details>

<summary>Delete Vinyl from the collection</summary>

Request: DELETE /api/Vinil/DeleteVinil/{id}

Response:
            
    {
     "success": true,
     "data": true,
     "message": "Vinil removido com sucesso!",
     "errors": null
    }

</details>


</details>
