CATATAN

1. Semua model dimasukkan ke folder yang telah ditentukan
2. Usahakan tidak memakai ViewModel sebagai response. Tetapi memakai anonymous type kecuali dipakai berkali kali
3. TIDAK HARUS memaksakan memakai LINQ utk proses database yang rumit. Gunakan stored procedure. 
   Alasan memakai linq adalah memudahkan bukan mempersulit termasuk apabila ada kemungkinan migrasi database misalnya sql server ke mariadb.
4. Tambahkan fluent api di dbcontext sesuai kebutuhan terutama utk composite primary key dan navigation property untuk menghindari inner join yang ribet
5. Semua logic dan perhitungan ada di service. Controller tidak melakukan logic dan perhitungan apapun.

How to use (case study)
Misalnya ada controller bernama AbcController untuk menghandle data pada tabel Abc di database maka harus dibuat:
1. Interface bernama IAbcService. 
	* Interface ini mengimplementasi IDataService<T> di mana T adalah model Abc.
	* Model Abc menghinherit class EntityBase untuk property tertentu. Abaikan jika tidak digunakan karena IDataService tidak mengikat terhadap EntityBase
2. Concrete class bernama AbcService mengimplementasi IAbcService terhadap model Abc
3. Register interface ini di class ServiceRegistrar di folder ContextAccessor
4. Inject IAbcService di controller AbcController.

public class AbcController : ApiController
{
	readonly IAbcService abc;
	public AbcController(IAbcService abc)
	{
		this.abc = abc;
	}
}