namespace PenggajianPegawai {
    public record Pegawai(
        string Nama,
        string Divisi,
        string Status,
        int HariKerja,
        int JamLembur 
    );

    class Program
    {
        private static Pegawai[] daftarPegawai = new Pegawai[5];
        private static int jumlahPegawai = 0;

        static void Main() {
            bool isRunning = true;
            while (isRunning) {
                Console.WriteLine("\n================================");
                Console.WriteLine("=  PROGRAM PENGGAJIAN PEGAWAI  =");
                Console.WriteLine("================================");
                Console.WriteLine("1. Input Data Pegawai           ");
                Console.WriteLine("2. Tampilkan Seluruh Pegawai    ");
                Console.WriteLine("3. Hitung Total Upah Pegawai    ");
                Console.WriteLine("4. Hitung Total Pengeluaran     ");
                Console.WriteLine("5. Keluar                       ");
                Console.WriteLine("================================");
                Console.Write("Pilih Menu: ");
                int.TryParse(Console.ReadLine(), out int inputMenu);
                
                switch (inputMenu) {
                    case 1:
                        InputDataPegawai();
                        break;
                    case 2:
                        TampilkanPegawai();
                        break;
                    case 3:
                        TampilkanDetailPegawai();
                        break;
                    case 4:
                        decimal totalPengeluaran = HitungTotalPengeluaran(daftarPegawai);
                        Console.WriteLine($"\nTotal pengeluaran perusahaan: Rp {totalPengeluaran:N0}");
                        break;
                    case 5:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid");
                        break;
                }
            }
        }

        public static void InputDataPegawai() {
            Console.WriteLine("\n================================");
            Console.WriteLine("=      INPUT DATA PEGAWAI      =");
            Console.WriteLine("================================");
            Console.Write("Jumlah Pegawai: ");
            int.TryParse(Console.ReadLine(), out int kapasitas);

            for (int i = 0; i < kapasitas; i++)
            {
                Console.WriteLine("\n--------------------------------");
                Console.WriteLine($"Data Pegawai ke-{i + 1}");
                Console.WriteLine("--------------------------------");
                
                Console.Write("Nama: ");
                string nama = Console.ReadLine() ?? "";

                Console.Write("Divisi (Produksi/Logistik/Administrasi): ");
                string divisi = Console.ReadLine() ?? "";

                Console.Write("Status (Tetap/Kontrak): ");
                string status = Console.ReadLine() ?? "";

                Console.Write("Hari Kerja: ");
                int hari = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Jam Lembur: ");
                int lembur = int.Parse(Console.ReadLine() ?? "0");

                if (jumlahPegawai >= daftarPegawai.Length)
                {
                    int kapasitasBaru = daftarPegawai.Length + 5;
                    Array.Resize(ref daftarPegawai, kapasitasBaru);
                }
    
                daftarPegawai[jumlahPegawai] = new Pegawai(nama, divisi, status, hari, lembur);
                jumlahPegawai++;
            }
        }

        private static (
            decimal TarifHarian,
            decimal UpahPokok, 
            decimal UpahLembur, 
            decimal Tunjangan, 
            decimal TotalUpah
        ) HitungTotalUpah(Pegawai pegawai) {
            decimal tarifHarian;
            if (pegawai.Status == "Tetap") {
                tarifHarian = 250_000m;
            }  else if (pegawai.Status == "Kontrak") {
                tarifHarian = 180_000m;
            } else {
                tarifHarian = 0m;  
            }
            decimal tarifPerJam = tarifHarian / 8;
            
            decimal upahPokok = pegawai.HariKerja * tarifHarian;
            decimal upahLembur;
            if (pegawai.JamLembur <= 0) {
                upahLembur = 0m;
            } else if (pegawai.JamLembur <= 8) {
                upahLembur = pegawai.JamLembur * 1.5m * tarifPerJam;
            } else {
                decimal upahLemburPertama = 8 * 1.5m * tarifPerJam;
                decimal upahLemburSetelahnya = (pegawai.JamLembur - 8) * 2.0m * tarifPerJam;
                upahLembur = upahLemburSetelahnya + upahLemburPertama;
            }

            decimal tunjangan; 
            if (pegawai.Divisi == "Produksi") {
                tunjangan = 500_000m;
            } else if (pegawai.Divisi == "Logistik") {
                tunjangan = 400_000m;
            } else if (pegawai.Divisi == "Administrasi") {
                tunjangan = 300_000m;
            } else {
                tunjangan = 0m;
            }

            decimal totalUpah = upahPokok + upahLembur + tunjangan;

            return (tarifHarian, upahPokok, upahLembur, tunjangan, totalUpah);
        }

    public static void TampilkanPegawai(){
        Console.WriteLine("\n=========================================================================================");
        Console.WriteLine($"{"Nama",-12} {"Divisi",-12} {"Status",-10} {"Hari",-5} {"Lembur",-6} {"Upah Pokok",-12} {"Upah Lembur",-12} {"Tunjangan",-12} {"Total",-12}");
        Console.WriteLine("=========================================================================================");

        for (int i = 0; i < jumlahPegawai; i++) {
            Pegawai pegawai = daftarPegawai[i];
            var upah = HitungTotalUpah(pegawai);
            Console.WriteLine($"{pegawai.Nama,-12} {pegawai.Divisi,-12} {pegawai.Status,-10} {pegawai.HariKerja,-5} {pegawai.JamLembur,-6} {upah.UpahPokok,-12:N0} {upah.UpahLembur,-12:N0} {upah.Tunjangan,-12:N0} {upah.TotalUpah,-12:N0}");
        }
    }

        private static void TampilkanDetailPegawai()
        {
            Console.Write("\nMasukkan Nama Pegawai: ");
            string pencarian = Console.ReadLine() ?? "";

            Pegawai? pegawai = daftarPegawai.FirstOrDefault(p => p.Nama.Equals(pencarian));

            if (pegawai == null) {
                Console.WriteLine("Pegawai tidak ditemukan.");
                return;
            }

            var hasil = HitungTotalUpah(pegawai);

            Console.WriteLine("\n================================");
            Console.WriteLine("=      DETAIL PERHITUNGAN      =");
            Console.WriteLine("================================");
            Console.WriteLine($"Nama        : {pegawai.Nama}");
            Console.WriteLine($"Divisi      : {pegawai.Divisi}");
            Console.WriteLine($"Status      : {pegawai.Status}");
            Console.WriteLine($"Hari Kerja  : {pegawai.HariKerja}");
            Console.WriteLine($"Jam Lembur  : {pegawai.JamLembur}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Tarif Harian: Rp {hasil.TarifHarian:N0}");
            Console.WriteLine($"Upah Pokok  : Rp {hasil.UpahPokok:N0}");
            Console.WriteLine($"Upah Lembur : Rp {hasil.UpahLembur:N0}");
            Console.WriteLine($"Tunjangan   : Rp {hasil.Tunjangan:N0}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Total Upah  : Rp {hasil.TotalUpah:N0}");
            Console.WriteLine("--------------------------------");
        }

        public static decimal HitungTotalPengeluaran(Pegawai[] daftarPegawai) {
            return daftarPegawai.Sum(p => HitungTotalUpah(p).TotalUpah);
        } 
    }
}
